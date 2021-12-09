using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class HovlStudiosSpawnProjectile : SpawnProjectile3D
{
    NetworkObject networkObj;
    override protected void Start()
    {
        networkObj = GetComponent<NetworkObject>();
        if (networkObj == null)
            networkObj = GetComponentInParent<NetworkObject>();

        base.Start();
    }
    // Update is called once per frame
    override protected void Update()
    {
        
        if ((AutoFire || ((networkObj ==null || networkObj.IsLocalPlayer) && (Input.GetAxis(InputAxisToInitateSpawnedObject) > 0) && (DateTime.Now - lastSpawnTime).TotalSeconds > secondsBetweenProjectiles)))
        {
            SpawnServerRpc();
        }
    }

    [ServerRpc]
    void SpawnServerRpc()
    {
        if (!IsServer)
            return;
        //are we within range of an object to shoot at?
        if (TagsOfObjectsToAttack.Length > 0)
        {
            bool foundOne = false;
            //are we within range of one of the specified objects to attack?
            foreach (string tag in TagsOfObjectsToAttack)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject o in objects)
                {
                    if (Vector3.Distance(gameObject.transform.position, o.transform.position) <= DistanceToStartAttacking)
                    {
                        foundOne = true;
                        continue;
                    }
                }
            }
            if (!foundOne)
                return;
        }

        //Debug.Log(rb.velocity.magnitude);
        //spawn the projectile
        GameObject projectile = Instantiate(projectTilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        NetworkObject netObj = projectile.GetComponent<NetworkObject>();
        if (netObj != null)
            netObj.Spawn(true);
        //add force to the projectile so it moves
        Rigidbody projectileRigidBody = projectile.GetComponent<Rigidbody>();

        ProjectileMover mover = projectile.GetComponent<ProjectileMover>();
        if (mover != null)
        {
            mover.baseSpeed = rb.velocity.magnitude + projectTileSpeed;
        }

        if (projectileRigidBody != null)
        {
            if (rb != null)
                projectileRigidBody.AddForce(spawnPoint.transform.forward * (projectTileSpeed + (rb.velocity.magnitude)), ForceMode.Impulse);
            else
                projectileRigidBody.AddForce(spawnPoint.transform.forward * (projectTileSpeed), ForceMode.Impulse);
        }
        //set the destroy time on the projectile
        Destroy(projectile, secondsBeforeProjectileDies);
        lastSpawnTime = DateTime.Now;

        //do we have a "fire" image to play?
        if (shotEffectsPrefab != null)
        {
            GameObject image = Instantiate(shotEffectsPrefab, spawnPoint.transform);
            NetworkObject netObj2 = image.GetComponent<NetworkObject>();
            if (netObj2 != null)
                netObj2.Spawn(true);
            Destroy(image, secondsBeforeShotEffectsDestroy);
        }
    }
}
