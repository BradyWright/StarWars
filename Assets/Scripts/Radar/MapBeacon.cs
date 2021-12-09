using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapBeacon : MonoBehaviour
{
	internal float mapX=0;
	internal float mapY=0;
	public GameObject RadarCenterObject;
	public string MapText = "";

    // Start is called before the first frame update
    void Start()
    {
        if (RadarCenterObject == null)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playerObjects)
            {
                NetworkObject no = player.GetComponentInParent<NetworkObject>();
                if (no.IsLocalPlayer)
                {
                    RadarCenterObject = player;
                }
            }
        }
        if (RadarCenterObject != null)
        {
            MapText = RadarCenterObject.name;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (RadarCenterObject != null)
        {
            //calculate distance from the player.
            mapX = gameObject.transform.position.x - RadarCenterObject.transform.position.x;
            mapY = gameObject.transform.position.z - RadarCenterObject.transform.position.z;
            //calculate the direction from the player
        }
	}
}
