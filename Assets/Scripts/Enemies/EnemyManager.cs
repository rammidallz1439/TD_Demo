using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyManager
{
    protected EnemyHandler Handler;

    #region EventHandlers
    protected void EnemySpawnEventHandler(EnemySpawnEvent e)
    {
        if (Handler.CoolDownTime <= 0)
        {
            foreach (EnemyData item in e.Wave.EnemyData)
            {
                if (item.TimeToStart <= 0)
                {
                    SpawnEnemies(item.EnemyScriptable.EnemyPrefab, item);
                }

                if (e.Timer <= e.Wave.WaveTime - item.TimeToStart)
                {
                    SpawnEnemies(item.EnemyScriptable.EnemyPrefab, item);
                }

            }
            Handler.CoolDownTime = 1f / Handler.SpawnRate;
        }
        Handler.CoolDownTime -= Time.deltaTime;
    }

    protected void EnemyMovementEventHandler(EnemyMovementEvent e)
    {
        e.Agent.SetDestination(Handler.House.position);
    }


    protected void FindTargetEventHandler(FindTargetEvent e)
    {
        if (e.ShootingMachine.Target == null)
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
                else
                {

                    e.ShootingMachine.Target = null;
                }
            }

            if (nearestEnemy != null && shortestDistance <= e.ShootingMachine.TurretDataScriptable.Range)
            {
                Enemy enemyComponent = nearestEnemy.GetComponent<Enemy>();

                if (enemyComponent != null)
                {
                    e.ShootingMachine.Target = enemyComponent;
                }
                else
                {
                    e.ShootingMachine.Target = null;
                }

            }
            else
            {
                e.ShootingMachine.Target = null;
            }
        }


    }

    #endregion

    #region Functions

    void SpawnEnemies(GameObject enemyObject, EnemyData data)
    {
        Collider cubeCollider = Handler.Platform.GetComponent<Collider>();

        float cubeMinX = cubeCollider.bounds.min.x;
        float cubeMaxX = cubeCollider.bounds.max.x;
        float spawnZ = Handler.EnemySpawnPoint.position.z;

        float randomX = Random.Range(cubeMinX, cubeMaxX);

        Vector3 spawnPosition = new Vector3(randomX, Handler.EnemySpawnPoint.position.y, spawnZ);
        GameObject enemy = MonoHelper.Instance.InstantiateObject(enemyObject, spawnPosition, Quaternion.identity);
        enemy.transform.GetComponent<Enemy>().Health = data.EnemyScriptable.Health;

    }



    #endregion
}
