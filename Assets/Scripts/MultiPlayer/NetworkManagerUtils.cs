using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkManagerUtils : NetworkBehaviour
{
 
    //NetworkManager networkManager;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    networkManager = FindObjectOfType<NetworkManager>();
    //    networkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    //}

    //private void NetworkManager_OnClientConnectedCallback(ulong obj)
    //{
    //    if (!networkManager.IsServer)
    //        return;

    //    ClientRpcParams clientRPCParams = new ClientRpcParams
    //    {
    //        Send = new ClientRpcSendParams
    //        {
    //            TargetClientIds = new ulong[] { obj }
    //        }
    //    };
    //    //send the current player names to the newly connected client
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    foreach (GameObject player in players)
    //    {
    //        NetworkObject no = player.GetComponent<NetworkObject>();
    //        if (no != null)
    //        {
    //            ulong netID = no.NetworkObjectId;
    //            ChangeObjectNameClientRpc(netID, player.name);
    //        }
    //    }
    //}


    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    //[ClientRpc]
    //void ChangeObjectNameClientRpc(ulong netID, string name)
    //{
    //    NetworkObject[] objects = FindObjectsOfType<NetworkObject>();
    //    for (int i = 0; i < objects.Length; i++)
    //    {
    //        if (objects[i].NetworkObjectId == netID)
    //        {
    //            objects[i].gameObject.name = name;
    //            MapBeacon mb = objects[i].GetComponent<MapBeacon>();
    //            if (mb != null)
    //                mb.MapText = name;
    //        }
    //    }
    //}


    //[ClientRpc]
    //private void InitGameClientRpc(ClientRpcParams parms)
    //{
        
    //}
}
