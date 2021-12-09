using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Aeroplane;
using Unity.Netcode;

[RequireComponent(typeof(AeroplaneController))]
public class MultiPlayerAirplaneControl2Axis : NetworkBehaviour
{
    // these max angles are only used on mobile, due to the way pitch and roll input are handled
    public float maxRollAngle = 80;
    public float maxPitchAngle = 80;

    // reference to the aeroplane that we're controlling
    private AeroplaneController m_Aeroplane;
    NetworkObject networkObject;


    private void Awake()
    {
        // Set up the reference to the aeroplane controller.
        m_Aeroplane = GetComponent<AeroplaneController>();
        networkObject = GetComponent<NetworkObject>();
    }


    private void FixedUpdate()
    {
        if (networkObject.IsLocalPlayer)
        {
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            float roll = CrossPlatformInputManager.GetAxis("Horizontal");
            float pitch = CrossPlatformInputManager.GetAxis("Vertical");
            bool airBrakes = CrossPlatformInputManager.GetButton("Fire2");

            // auto throttle up, or down if braking.
            float throttle = airBrakes ? -1 : 1;
#if MOBILE_INPUT
            AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);
#endif
            // Pass the input to the aeroplane
            MovePlaneServerRpc(roll, pitch, 0, throttle, airBrakes);
        }
    }

    [ServerRpc]
    private void MovePlaneServerRpc(float roll, float pitch, float yaw, float throttle, bool airBrakes)
    {
        if (!IsServer)
            return;

        m_Aeroplane.Move(roll, pitch, yaw, throttle, airBrakes);
    }


    private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
    {
        // because mobile tilt is used for roll and pitch, we help out by
        // assuming that a centered level device means the user
        // wants to fly straight and level!

        // this means on mobile, the input represents the *desired* roll angle of the aeroplane,
        // and the roll input is calculated to achieve that.
        // whereas on non-mobile, the input directly controls the roll of the aeroplane.

        float intendedRollAngle = roll * maxRollAngle * Mathf.Deg2Rad;
        float intendedPitchAngle = pitch * maxPitchAngle * Mathf.Deg2Rad;
        roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
        pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);

        // similarly, the throttle axis input is considered to be the desired absolute value, not a relative change to current throttle.
        float intendedThrottle = throttle * 0.5f + 0.5f;
        throttle = Mathf.Clamp(intendedThrottle - m_Aeroplane.Throttle, -1, 1);
    }
}
