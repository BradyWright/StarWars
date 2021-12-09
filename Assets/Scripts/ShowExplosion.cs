using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowExplosion : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SmallRandomExplosions()
    {
        GameObject projectile = Instantiate(ExplosionPrefab, gameObject.transform.position, gameObject.transform.rotation);

    }

    public void MeshExplode()
    {
        BroadcastMessage("Explode");
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
