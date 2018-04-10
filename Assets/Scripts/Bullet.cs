﻿using UnityEngine.Networking;
using UnityEngine;

public class Bullet : NetworkBehaviour {

    private NetworkIdentity owner;
    [SerializeField] private int bulletDamage;
    [SerializeField] private GameObject bloodEffectPrefab;
    [SerializeField] private GameObject dustEffectPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    private Quaternion particleRotation;

    public void SetOwner(NetworkIdentity id)
    {
        owner = id;
    }

    // Updates every frame.
    void Update()
    {   
        // Let the clients update the bullets instead of server.
        if (!isClient)
            return;
        RaycastHit hit;
        // First check if the bullet hits anything this frame.
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletSpeed * Time.deltaTime))
        {
            HitDetection(hit);
        }

        // If no hit this frame. Move bullet forward.
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        // Destroy bullet after 5 seconds. 
        Destroy(gameObject, bulletLifeTime);
    }

    // Tell the server to update the player that got hit's health.
    [Command]
    void CmdPlayerHit(int _damage, GameObject _player)
    {
        _player.GetComponent<PlayerHealth>().RpcTakeDamage(_damage);
    }

    // Tell the server to spawn a particleeffect on a position with a rotation.
    [Command]
    void CmdSpawnParticle(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject effect = Instantiate(prefab, position, rotation);
        NetworkServer.Spawn(effect);
        Destroy(effect, 0.5f);
    }
    void HitDetection(RaycastHit hit)
    {
        particleRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        // If the bullet hits a player. Create blood effect on hit.position.
        if (hit.collider.gameObject.CompareTag("Player"))
        {
            CmdSpawnParticle(bloodEffectPrefab, hit.point, particleRotation);
            CmdPlayerHit(bulletDamage, hit.collider.gameObject);
            Destroy(gameObject);
        }
        else // Otherwise play dust particles on hit.position. 
        {
            CmdSpawnParticle(dustEffectPrefab, hit.point, particleRotation);
            Destroy(gameObject);
        }
    }

}
