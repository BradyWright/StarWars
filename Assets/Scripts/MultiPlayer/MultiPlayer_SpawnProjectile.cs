using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiPlayer_SpawnProjectile : SpawnProjectile3D
{
    // Start is called before the first frame update
    new void Start()
    {
        if (NetworkManager.Singleton.IsClient)
            base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        if (NetworkManager.Singleton.IsClient)
            base.Update();
    }
}
