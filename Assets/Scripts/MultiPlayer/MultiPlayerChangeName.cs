using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Collections;

public class MultiPlayerChangeName : NetworkBehaviour
{

    //public Text playerName; //the tag that floats above your head
    //public GameObject Player; //the player gameobject
    //public InputField InputField; //the input field

    public void TestChangeName()
    {
        this.gameObject.name = System.Guid.NewGuid().ToString();
        NetworkObject no = GetComponent<NetworkObject>();
        ChangePlayerName(no.NetworkObjectId, this.gameObject.name);
    }

    [ServerRpc]
    public void SyncPlayerNamesServerRpc()
    {
        if (IsServer)
        {
            foreach (MultiPlayer_PlayerOnStartSetup player in FindObjectsOfType<MultiPlayer_PlayerOnStartSetup>())
            {
                player.playerName.SetDirty(true);
            }
        }
    }

    public void ChangePlayerName(ulong NetworkObjectId, string newName)
    {
        //called when the inputfield has been modifed from the UI
        //CmdSendName(playerName.text);
        //NetworkObject no = GetComponent<NetworkObject>();
        CmdSendNameServerRpc(NetworkObjectId, newName);
    }

    //[ClientRpc]
    //void RpcOnNameChangedClientRpc(ulong netID,string name)
    //{
    //    NetworkObject[] objects = FindObjectsOfType<NetworkObject>();
    //    for(int i=0;i<objects.Length;i++)
    //    {
    //        if(objects[i].NetworkObjectId == netID)
    //        {
    //            objects[i].gameObject.name = name;
    //            MapBeacon mb = objects[i].GetComponent<MapBeacon>();
    //            if (mb != null)
    //                mb.MapText = name;
    //        }
    //    }
    //}

    [ServerRpc]
    void CmdSendNameServerRpc(ulong netID,string name)
    {
        //a client tells the server to update the name
        //tell other clients to update the name change
        //RpcOnNameChangedClientRpc(netID,name);
        if (IsServer)
        {
            NetworkObject[] objects = FindObjectsOfType<NetworkObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].NetworkObjectId == netID)
                {
                    MultiPlayer_PlayerOnStartSetup ss = objects[i].GetComponent<MultiPlayer_PlayerOnStartSetup>();
                    ss.playerName.Value = new FixedString64Bytes(name);
                }
            }
        }
    }

    //[ServerRpc]
    //public void GetNameServerRpc(ulong netID)
    //{
    //    NetworkObject[] objects = FindObjectsOfType<NetworkObject>();
    //    for (int i = 0; i < objects.Length; i++)
    //    {
    //        if (objects[i].NetworkObjectId == netID)
    //        {
    //            RpcOnNameChangedClientRpc(objects[i].NetworkObjectId,objects[i].gameObject.name);
    //        }
    //    }
    //}
}