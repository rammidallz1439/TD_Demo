using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMachine : MonoBehaviour
{
    public Transform PartToRotate;
    public Transform SpawnPoint;
    public Enemy Target = null;

    public TurretDataScriptable TurretDataScriptable;
    private ITurretAction turretAction;

    public float CoolDown;
    public float FireRate;

    [Space(10)]
    [Header("Laser Specific")]
    public Bullet LaserPointer = null;


    private void Start()
    {
        CheckTurretType();
    }
    private void Update()
    {
        if (turretAction is not null)
            turretAction.Excute();
    }

    private void CheckTurretType()
    {
        if (TurretDataScriptable is BulletShooter shooter)
        {
            turretAction = new BulletShooterAction(this);
        }
        else if (TurretDataScriptable is RocketShooter rocket)
        {
            turretAction = new RocketShooterAction(this);
        }
        else if (TurretDataScriptable is LaserShooter laser)
        {
            turretAction = new LaserShooterAction(this);
        }
    }
}
