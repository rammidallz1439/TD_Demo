using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretManager
{
    protected TurretHandler Handler;
    GameObject ammo;
    #region Event Handler
    protected void BulletFireEventHandler(BulletFireEvent e)
    {
        if (e.ShootingMachine.CoolDown <= 0)
        {
            if (e.ShootingMachine.Target != null && e.ShootingMachine.TurretDataScriptable != null)
            {
                if (e.ShootingMachine.TurretDataScriptable is RocketShooter rocket)
                {
                    ammo = MonoHelper.Instance.InstantiateObject(e.ShootingMachine.TurretDataScriptable.Bullet.gameObject);
                }
                else
                {
                    ammo = Vault.ObjectPoolManager.Instance.Get(e.ShootingMachine.TurretDataScriptable.Bullet.gameObject.name, true);

                }
                ammo.transform.GetComponent<Bullet>().Target = e.ShootingMachine.Target.transform;
                ammo.transform.position = e.ShootingMachine.SpawnPoint.position;
                ammo.transform.GetComponent<Bullet>().AttackPower = e.ShootingMachine.TurretDataScriptable.AttackPower;
            }
            e.ShootingMachine.CoolDown = 1f / e.ShootingMachine.FireRate;
        }
        e.ShootingMachine.CoolDown -= Time.deltaTime;
    }

    protected void LookAtTargetEventHandler(LookAtTargetEvent e)
    {
        if (e.ShootingMachine.Target == null)
            return;

        if (e.ShootingMachine.Target != null)
        {
            if (e.ShootingMachine.Target.gameObject.activeSelf)
            {
                Vector3 direction = e.ShootingMachine.Target.transform.position - e.ShootingMachine.PartToRotate.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                if (e.ShootingMachine.PartToRotate is not null)
                {
                    Vector3 rotation = Quaternion.Lerp(e.ShootingMachine.PartToRotate.rotation, lookRotation, Time.deltaTime * Handler.RotationSpeed).eulerAngles;
                    e.ShootingMachine.PartToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
                }
            }

        }

    }

    //Moves Bullet to the Enemy
    protected void BulletEventhandler(BulletEvent e)
    {
        if (e.Target == null)
            MonoHelper.Instance.DestroyObject(e.Current);

        if (e.Target != null)
        {
            Vector3 additionalHeight = e.Target.position + new Vector3(0, 1.5f, 0);
            Vector3 direction = (additionalHeight - e.Current.transform.position).normalized;
            float distanceThisFrame = e.Speed * Time.deltaTime;
            e.Current.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }

    }

    // Moves Rocket in a Parabolic trajectory
    protected void RocketEventHandler(RocketEvent e)
    {
        Bullet bullet = e.Current.transform.GetComponent<Bullet>();

        bullet.velocity += new Vector3(0, e.Gravity * Time.deltaTime, 0); 

        e.Current.transform.position += bullet.velocity * Time.deltaTime;

        if (bullet.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            e.Current.transform.rotation = Quaternion.LookRotation(bullet.velocity);
        }
    }

    //Blasts All Enemies in Range of Blast
    protected void RocketBlastEventHandler(RocketBlastEvent e)
    {
        Collider[] colliders = Physics.OverlapSphere(e.Current.transform.position, e.BlastRadius);
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(e.AttackPower);
                GameObject effect = MonoHelper.Instance.InstantiateObject(e.Current.transform.GetComponent<Bullet>().BlastEffect,
                    e.Current.transform.position, Quaternion.identity);
                effect.transform.GetComponent<ParticleSystem>().Play();
                MonoHelper.Instance.DestroyObject(effect, 1f);
            }
        }
    }

    //Claculates the trajectory for Rocket Launcher
    protected void CalculateRocketVelocityEventHandler(CalcualteRocketVelocityEvent e)
    {

        if (e.BulletType == BulletType.Rocket && e.Current.Target != null)
        {
            Vector3 direction = e.Current.Target.transform.position - e.Current.transform.position;
            direction.y = 0;
            float distance = direction.magnitude;
            float angle = e.LaunchAngle * Mathf.Deg2Rad;

            float velocityMagnitude = Mathf.Sqrt((distance * Mathf.Abs(e.Gravity)) / Mathf.Sin(2 * angle));

            Vector3 velocityY = Vector3.up * velocityMagnitude * Mathf.Sin(angle);
            Vector3 velocityXZ = direction.normalized * velocityMagnitude * Mathf.Cos(angle);

            e.Current.transform.GetComponent<Bullet>().velocity = velocityXZ + velocityY;
        }
    }


    protected void LaserShootEventHandler(LaserShootEvent e)
    {
        if (e.ShootingMachine.CoolDown <= 0)
        {
            if (e.ShootingMachine.Target != null && e.ShootingMachine.TurretDataScriptable != null)
            {
                if (e.ShootingMachine.LaserPointer != null)
                {
                    e.ShootingMachine.LaserPointer.gameObject.SetActive(true);
                    e.ShootingMachine.LaserPointer.transform.position = e.ShootingMachine.Target.transform.position + new Vector3(0, 1.5f, 0);
                    e.ShootingMachine.LaserPointer.AttackPower = e.ShootingMachine.TurretDataScriptable.AttackPower;
                    e.ShootingMachine.LaserPointer.Target = e.ShootingMachine.Target.transform;
                }

            }
            e.ShootingMachine.CoolDown = 1f / e.ShootingMachine.FireRate;
        }
        e.ShootingMachine.CoolDown -= Time.deltaTime;
    }

    #endregion

    #region Functions


    #endregion
}
