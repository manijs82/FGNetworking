using Unity.Netcode;
using UnityEngine;

public class RespawnMenu : NetworkBehaviour
{
    public void Toggle(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    public void Respawn()
    {
        NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = Vector3.zero;
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Health>().UnFreezeRpc();
        Toggle(false);
    }
}