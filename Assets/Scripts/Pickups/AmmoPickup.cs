using Unity.Netcode;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            FiringAction firingAction = other.GetComponent<FiringAction>();
            if (!firingAction) return;
            firingAction.RechargeAmmo();

            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}