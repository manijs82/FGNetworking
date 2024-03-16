using Unity.Netcode;
using UnityEngine;

public class HealthPickup : NetworkBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            Health health = other.GetComponent<Health>();
            if (!health) return;
            health.AddHealth(25);

            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}