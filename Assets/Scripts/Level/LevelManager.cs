using DG.Tweening;
using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    protected LevelHandler Handler;

    #region EventHandler
    protected void InitialLevelSetupEventHandler(IntialLevelSetUpEvent e)
    {
        Handler.CurrentWave = Handler.LevelDetails.WaveData.Waves[Handler.CurrentWaveCount - 1];
        Handler.WaveCount.text = "Wave: " + Handler.CurrentWaveCount + "/" + Handler.LevelDetails.WaveData.Waves.Count;
        Handler.CoinCount.text = GlobalManager.Instance.TotalCoins.ToString();
        Handler.Timer = Handler.CurrentWave.WaveTime;
        Handler.SpawnRate = Handler.CurrentWave.EnemiesSpeed;
        Handler.RestartButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
        Handler.PlayAgainButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
    }

    protected void BaseSelectedEventHandler(BaseSelectedEvent e)
    {
        if (Handler.CurrentSelectedBase == null)
        {
            Handler.CurrentSelectedBase = e.BaseHandler;
        }
        {
            Handler.CurrentSelectedBase.transform.DOMoveY(Handler.CurrentSelectedBase.InitialYPos, 0.5f);
            if (Handler.CurrentSelectedBase.Occupied == false)
                Handler.CurrentSelectedBase.transform.GetComponent<MeshRenderer>().material.color = Handler.CurrentSelectedBase.GetColor();
            Handler.CurrentSelectedBase = e.BaseHandler;

        }

        e.BaseHandler.transform.DOMoveY(1f, 0.5f);
        e.BaseHandler.transform.GetComponent<MeshRenderer>().material.color = Color.green;
        Handler.SelectionPanel.gameObject.SetActive(true);
    }

    protected void SpawnTurretEventHandler(SpawnTurretEvent e)
    {
        if (Handler.CurrentSelectedBase != null && Handler.CurrentSelectedBase.Occupied == false)
        {
            Handler.CurrentSelectedBase.transform.DOMoveY(Handler.CurrentSelectedBase.InitialYPos, 0.5f).OnComplete(() =>
            {
                GameObject obj = MonoHelper.Instance.InstantiateObject(e.Turret, Handler.CurrentSelectedBase.SpawnPoint.transform.position, Quaternion.identity);
                Handler.CurrentSelectedBase.Occupied = true;
                ShootingMachine machine = e.Turret.transform.GetComponent<ShootingMachine>();
                machine.Range = machine.TurretDataScriptable.Range;
                Vault.ObjectPoolManager.Instance.InitializePool(machine.TurretDataScriptable.Bullet.gameObject, 15);
            });

        }
    }

    protected void UpdateTimerEventHandler(UpdateTimerEvent e)
    {
        if (Handler.CurrentWaveCount <= Handler.LevelDetails.WaveData.Waves.Count)
        {
            if (Handler.Timer > 0)
            {
                Handler.Timer -= Time.deltaTime;
                UpdateTimerDisplay(Handler.Timer);
            }
            else
            {

                Handler.Timer = 0;
                TimerEnded();
                UpdateTimerDisplay(Handler.Timer);
            }

        }

    }

    protected void CoinDobberAnimationHandler(CoinDobberAnimation e)
    {
        Vector3 coinScreenPosition = Camera.main.WorldToScreenPoint(e.CoinPrefab.transform.position);

        Vector3 targetScreenPosition = Handler.CoinCount.rectTransform.position;

        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetScreenPosition.x, targetScreenPosition.y, coinScreenPosition.z));

        Timing.RunCoroutine(DelayCoinsDeath(e.CoinPrefab, targetWorldPosition).CancelWith(e.CoinPrefab));
    }


    #endregion

    #region Functions
    IEnumerator<float> DelayCoinsDeath(GameObject coin, Vector3 targetWorldPosition)
    {
        yield return Timing.WaitForSeconds(0.5f);
        coin.transform.DOMove(targetWorldPosition, 0.5f)
              .SetEase(Ease.Linear)
              .OnComplete(() =>
              {
                  Handler.CoinCount.text = GlobalManager.Instance.TotalCoins.ToString();
                  MonoHelper.Instance.DestroyObject(coin);
              })
              .WaitForCompletion();
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        Handler.TimerCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {


        if (Handler.CurrentWaveCount >= Handler.LevelDetails.WaveData.Waves.Count)
        {
            Enemy[] enemies = Object.FindObjectsOfType<Enemy>();
            if (enemies.Length > 0)
            {
                foreach (Enemy item in enemies)
                {
                    MonoHelper.Instance.DestroyObject(item.gameObject);
                }
            }

            Handler.LevelCompleted = true;
            Handler.LevelSucessPanel.SetActive(true);
            Handler.Timer = 0;
            UpdateTimerDisplay(Handler.Timer);
        }
        else
        {
            Handler.CurrentWaveCount++;
            Handler.WaveCount.text = "Wave: " + Handler.CurrentWaveCount + "/" + Handler.LevelDetails.WaveData.Waves.Count;
            Handler.CurrentWave = Handler.LevelDetails.WaveData.Waves[Handler.CurrentWaveCount - 1];
            Handler.Timer = Handler.CurrentWave.WaveTime;
            Handler.SpawnRate = Handler.CurrentWave.EnemiesSpeed;
        }
    }
    #endregion
}
