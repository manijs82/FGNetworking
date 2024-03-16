using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
       
    public NetworkVariable<int> ammoCount = new NetworkVariable<int>(10);
    private NetworkVariable<float> ammoTimer = new NetworkVariable<float>(0);
    private NetworkVariable<bool> canShoot = new NetworkVariable<bool>(true);

    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
    }

    private void Fire(bool isShooting)
    {
        if (isShooting && canShoot.Value && ammoCount.Value > 0)
        {
            ShootLocalBullet();
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        if (!canShoot.Value)
        {
            if (ammoTimer.Value <= 0)
            {
                canShoot.Value = true;
            }

            ammoTimer.Value -= Time.deltaTime;
        }
    }

    public void RechargeAmmo()
    {
        ammoCount.Value = 10;
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletClientRpc();

        if (ammoCount.Value > 0)
        {
            canShoot.Value = false;
            ammoTimer.Value = 1;
            ammoCount.Value--;
        }
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
    }

    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }
}
