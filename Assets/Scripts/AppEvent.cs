using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vault;

public class AppEvent
{
}


public struct EnemySpawnEvent : GameEvent
{
    public Wave Wave;
    public float Timer;

    public EnemySpawnEvent(Wave wave, float timer)
    {
        Wave = wave;
        Timer = timer;
    }
}

public struct IntialLevelSetUpEvent : GameEvent
{

}

public struct EnemyMovementEvent : GameEvent
{
    public NavMeshAgent Agent;

    public EnemyMovementEvent(NavMeshAgent agent)
    {
        Agent = agent;
    }
}

public struct MakeGridEvent : GameEvent
{

}

public struct BaseSelectedEvent : GameEvent
{
    public BaseHandler BaseHandler;

    public BaseSelectedEvent(BaseHandler baseHandler)
    {
        BaseHandler = baseHandler;
    }
}

public struct SpawnTurretEvent : GameEvent
{
    public GameObject Turret;

    public SpawnTurretEvent(GameObject turret)
    {
        Turret = turret;
    }
}

public struct FindTargetEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public FindTargetEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct LookAtTargetEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public LookAtTargetEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct BulletFireEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public BulletFireEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct UpdateTimerEvent : GameEvent
{

}

public struct BulletEvent : GameEvent
{
    public Transform Target;
    public GameObject Current;
    public float Speed;

    public BulletEvent(Transform target, GameObject current, float speed)
    {
        Target = target;
        Current = current;
        Speed = speed;
    }
}

public struct RocketEvent : GameEvent
{
    public Transform Target;
    public GameObject Current;
    public float Speed;
    public float BlastRadius;
    public float Gravity;

    public RocketEvent(Transform target, GameObject current, float speed, float blastRadius, float gravity)
    {
        Target = target;
        Current = current;
        Speed = speed;
        BlastRadius = blastRadius;
        Gravity = gravity;
    }
}

public struct RocketBlastEvent : GameEvent
{
    public float AttackPower;
    public GameObject Current;
    public float BlastRadius;

    public RocketBlastEvent(float attackPower, GameObject current, float blastRadius)
    {
        AttackPower = attackPower;
        Current = current;
        BlastRadius = blastRadius;
    }
}

public struct CalcualteRocketVelocityEvent : GameEvent
{
    public BulletType BulletType;
    public Bullet Current;
    public float LaunchAngle;
    public float Gravity;

    public CalcualteRocketVelocityEvent(BulletType bulletType, Bullet current, float launchAngle, float gravity)
    {
        BulletType = bulletType;
        Current = current;
        LaunchAngle = launchAngle;
        Gravity = gravity;
    }
}


public struct CoinDobberAnimation : GameEvent 
{
    public GameObject CoinPrefab;
    public CoinDobberAnimation(GameObject coinPrefab)
    {
        CoinPrefab = coinPrefab;
    }
}

public struct LaserShootEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public LaserShootEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}