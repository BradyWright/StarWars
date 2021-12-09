using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class MultiPlayer_PlayerOnStartSetup : NetworkBehaviour
{
    [SerializeField]
    public Unity.Netcode.NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>();

    MultiPlayerMenu menu;
    NetworkObject networkObject;

    void OnEnable()
    {
        playerName.OnValueChanged += OnValueChanged;
       
    }

    void OnDisable()
    {
        playerName.OnValueChanged -= OnValueChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        //attach the virtual camera to the network connected player
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        menu = GameObject.FindObjectOfType<MultiPlayerMenu>();
        networkObject = GetComponentInParent<NetworkObject>();

        if (IsLocalPlayer)
        {
            //update the cinemachine.
            Cinemachine.CinemachineVirtualCamera cvm = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            if (cvm != null)
            {
                cvm.Follow = gameObject.transform;
                cvm.LookAt = gameObject.transform;
            }


            MapBeacon[] beacons = GameObject.FindObjectsOfType<MapBeacon>();
            foreach (MapBeacon beacon in beacons)
            {
                beacon.RadarCenterObject = gameObject;
            }
            //point the minimap at the player
            Minimap minimap = GameObject.FindObjectOfType<Minimap>();
            if (minimap != null)
            {
                minimap.playerObject = gameObject;
                minimap.Radar = GetComponentInParent<Radar>();
                if (menu != null)
                    minimap.PlayerName = menu.PlayerName;
            }

            //GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);

        }


       
        

    }
    
    public void OnValueChanged(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        this.gameObject.name = newValue.ToString();
        if (IsClient && !IsLocalPlayer)
        {            
                MapBeacon mb = GetComponent<MapBeacon>();
                mb.MapText = newValue.ToString();   
        }
    }

    // Update is called once per frame
    void Update()
    {
        //    if((System.DateTime.Now-timeSinceLastNameUpdateCheck).Seconds>10)
        //    {
        //        if (IsOwner)
        //        {
        //            playerName.Value = new FixedString64Bytes(System.Guid.NewGuid().ToString());
        //        }
        //        timeSinceLastNameUpdateCheck = System.DateTime.Now;

        //    }

        if (menu != null && networkObject != null && this.tag == "Player" && networkObject.IsLocalPlayer && this.gameObject.name != menu.PlayerName)
        {
            ulong id = networkObject.NetworkObjectId;
            string name = menu.PlayerName;
            GetComponent<MultiPlayerChangeName>().ChangePlayerName(id, name);
        }
    }
}
