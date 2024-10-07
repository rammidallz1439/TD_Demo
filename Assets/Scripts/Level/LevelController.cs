using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class LevelController : LevelManager, IController, ITick
{
    public LevelController(LevelHandler handler)
    {
        Handler = handler;
    }
    public void OnInitialized()
    {
        EventManager.Instance.TriggerEvent(new IntialLevelSetUpEvent());
    }

    public void OnRegisterListeners()
    {
        EventManager.Instance.AddListener<IntialLevelSetUpEvent>(InitialLevelSetupEventHandler);
        EventManager.Instance.AddListener<BaseSelectedEvent>(BaseSelectedEventHandler);
        EventManager.Instance.AddListener<SpawnTurretEvent>(SpawnTurretEventHandler);
        EventManager.Instance.AddListener<UpdateTimerEvent>(UpdateTimerEventHandler);
        EventManager.Instance.AddListener<CoinDobberAnimation>(CoinDobberAnimationHandler);
    }

    public void OnRelease()
    {
    }

    public void OnRemoveListeners()
    {
        EventManager.Instance.RemoveListener<IntialLevelSetUpEvent>(InitialLevelSetupEventHandler);
        EventManager.Instance.RemoveListener<BaseSelectedEvent>(BaseSelectedEventHandler);
        EventManager.Instance.RemoveListener<SpawnTurretEvent>(SpawnTurretEventHandler);
        EventManager.Instance.RemoveListener<UpdateTimerEvent>(UpdateTimerEventHandler);
        EventManager.Instance.RemoveListener<CoinDobberAnimation>(CoinDobberAnimationHandler);

    }

    public void OnStarted()
    {
    }

    public void OnUpdate()
    {
        EventManager.Instance.TriggerEvent(new UpdateTimerEvent());
        if (!Handler.LevelCompleted)
            EventManager.Instance.TriggerEvent(new EnemySpawnEvent(Handler.CurrentWave, Handler.Timer));
        MonoHelper.Instance.FaceCamera(Handler.Camera, Handler.HouseSlider.transform);
    }

    public void OnVisible()
    {
    }
}
