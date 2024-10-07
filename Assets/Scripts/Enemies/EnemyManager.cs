using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    protected EnemyHandler Handler;

    #region EventHandlers
    protected void EnemySpawnEventHandler(EnemySpawnEvent e)
    {
        if (Handler.CoolDownTime <= 0)
        {
            EnemyData data = e.Wave.EnemyData[Random.Range(0, e.Wave.EnemyData.Count)];
            SpawnEnemies(data.EnemyScriptable.EnemyPrefab, data);
            Handler.CoolDownTime = 1f / e.Speed;
        }
        Handler.CoolDownTime -= Time.deltaTime;
    }

    protected void EnemyMovementEventHandler(EnemyMovementEvent e)
    {
        if (e.Enemy.CurrentPoint < Handler.Path.PathPoints.Count)
        {
            if (Vector3.Distance(e.Enemy.transform.position, Handler.Path.PathPoints[e.Enemy.CurrentPoint].position) < 0.5f)
            {
                e.Enemy.CurrentPoint++;
                if (e.Enemy.CurrentPoint >= Handler.Path.PathPoints.Count)
                {
                    return;
                }
            }
            e.Enemy.transform.position = Vector3.MoveTowards(e.Enemy.transform.position, Handler.Path.PathPoints[e.Enemy.CurrentPoint].position, e.Enemy.Speed * Time.deltaTime);
        }

    }


    protected void FindTargetEventHandler(FindTargetEvent e)
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(e.ShootingMachine.transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
       
        }


        if (nearestEnemy != null && shortestDistance <= e.ShootingMachine.Range)
        {
            e.ShootingMachine.Target = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            e.ShootingMachine.Target = null;
        }
    }

    #endregion

    #region Functions

    void SpawnEnemies(GameObject enemyObject, EnemyData data)
    {
        Vector3 spawnPosition = Handler.Path.PathPoints[0].position;
        GameObject enemy = MonoHelper.Instance.InstantiateObject(enemyObject, spawnPosition, Quaternion.identity);
        enemy.transform.GetComponent<Enemy>().Health = data.EnemyScriptable.Health;
    }



    #endregion
}
