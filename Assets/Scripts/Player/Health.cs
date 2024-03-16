using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        currentHealth.Value = maxHealth;
        currentHealth.OnValueChanged += OnHealthChange;
    }

    private void OnHealthChange(int previousValue, int newValue)
    {
        if(currentHealth.Value <= 0) OnDeath();
    }

    public void TakeDamage(int damage)
    {
        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);
    }
    
    public void AddHealth(int amount)
    {
        currentHealth.Value += amount;
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);
    }

    private void OnDeath()
    {
        MoveAwayRpc();
        FreezeRpc();
        ToggleRespawnPanelRpc(true);
    }

    [Rpc(SendTo.Everyone)]
    public void ToggleRespawnPanelRpc(bool active)
    {
        if(!IsOwner) return;
        FindObjectOfType<RespawnMenu>().Toggle(active);
    }
    
    [Rpc(SendTo.Everyone)]
    public void MoveAwayRpc()
    {
        if(!IsOwner) return;
        transform.position = new Vector3(100, 0);
    }

    [Rpc(SendTo.Server)]
    public void FreezeRpc()
    {
        GetComponent<PlayerController>().canMove.Value = false;
        GetComponent<PlayerController>().canFire.Value = false;
    }
    
    [Rpc(SendTo.Server)]
    public void UnFreezeRpc()
    {
        GetComponent<PlayerController>().canMove.Value = true;
        GetComponent<PlayerController>().canFire.Value = true;
        currentHealth.Value = maxHealth;
    }
}