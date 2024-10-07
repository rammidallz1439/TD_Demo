using Vault;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContextController : Registerer
{
    [SerializeField] private EnemyHandler _enemyHandler;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private TurretHandler _turretHandler;
    public override void Enable()
    {
    }

    public override void OnAwake()
    {
        AddController(new EnemyController(_enemyHandler));
        AddController(new LevelController(_levelHandler));
        AddController(new TurretController(_turretHandler));
    }

    public override void OnStart()
    {
    }
}
