using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Shotgun : Guns
{
    public float fovAngle = 60f;
    public float fovRange = 5f;
    public float shotgunDamageMultiplier = 10f;
    public bool hasShot = false;
    public Transform shotgunTip;
    public LayerMask enemyLayer;
    public GameObject particleBullets;

    void Awake()
    {
        GunDamage = 1f;
    }

    public void Shoot(bool facingRight, float damageMultiplier) 
    {
        if (facingRight) 
        {
            Instantiate(particleBullets, (shotgunTip.position + new Vector3(0.8f, 0, 0)), Quaternion.Euler(0, 90, 0));
        }
        else
        {
            Instantiate(particleBullets, (shotgunTip.position + new Vector3(-0.8f, 0, 0)), Quaternion.Euler(0, -90, 0));
        }

        float tempGunDamage = GunDamage;

        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Collider2D[] circleRange = Physics2D.OverlapCircleAll(shotgunTip.position, fovRange, enemyLayer);
        foreach (Collider2D hit in circleRange)
        {
            Enemy enemyScript = hit.GetComponent<Enemy>();

            Vector2 directionToEnemy = (hit.transform.position - shotgunTip.position).normalized;
            float angleToEnemy = Vector2.Angle(direction, directionToEnemy);

            if (angleToEnemy < fovAngle / 2)
            {
                shotgunDamageMultiplier = damageMultiplier;
                float distanceToEnemy = Vector3.Distance(hit.transform.position, shotgunTip.position);
                switch (distanceToEnemy)
                {
                    case < 2f:
                        GunDamage *= 9f * shotgunDamageMultiplier;
                        break;
                    case < 3.5f:
                        GunDamage *= 6f * shotgunDamageMultiplier;
                        break;
                    case < 4.3f:
                        GunDamage *= 4f * shotgunDamageMultiplier;
                        break;
                    case <= 5.5f:
                        GunDamage *= 1f * shotgunDamageMultiplier;
                        break;
                }
                if(enemyScript != null)
                {
                    enemyScript.TakeDamage(GunDamage);
                }
                hasShot = true;
                GunDamage = tempGunDamage;
            }
        }

        if (!hasShot)
        {
            Collider2D[] closeupRange = Physics2D.OverlapCircleAll(shotgunTip.position + new Vector3(-0.35f, 0, 0), 0.8f, enemyLayer);
            foreach (Collider2D hit in closeupRange)
            {
                Enemy enemyScript = hit.GetComponent<Enemy>();

                Vector2 directionToEnemy = facingRight ? (hit.transform.position - shotgunTip.position - new Vector3(-0.35f, 0, 0)).normalized :
                    (hit.transform.position - shotgunTip.position + new Vector3(-0.35f, 0, 0)).normalized;
                float angleToEnemy = Vector2.Angle(direction, directionToEnemy);

                if (angleToEnemy < 90f / 2)
                {
                    GunDamage *= 10f;
                    enemyScript.TakeDamage(GunDamage);
                    GunDamage = tempGunDamage;
                }
            }
        }
        hasShot = false;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, Vector3.forward) * transform.right * fovRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, Vector3.forward) * transform.right * fovRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(shotgunTip.position, fovLine1);
        Gizmos.DrawRay(shotgunTip.position, fovLine2);

        Vector3 closeupLine1 = Quaternion.AngleAxis(90f / 2, Vector3.forward) * transform.right * 0.8f;
        Vector3 closeupLine2 = Quaternion.AngleAxis(-90f / 2, Vector3.forward) * transform.right * 0.8f;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(shotgunTip.position + new Vector3(-0.35f, 0, 0), closeupLine1);
        Gizmos.DrawRay(shotgunTip.position + new Vector3(-0.35f, 0, 0), closeupLine2);
    }

}
