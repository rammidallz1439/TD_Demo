using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    [Header("Level Details")]
    public LevelScriptable LevelDetails;
    public Wave CurrentWave = null;
    public Camera Camera;

    [Space(10)]
    [Header("Data")]
    public float Timer;
    public int CurrentWaveCount;
    public bool LevelCompleted;
    public float SpawnRate;

    [Space(10)]
    [Header("UI")]
    public TMP_Text TimerCount;
    public TMP_Text WaveCount;
    public TMP_Text CoinCount;
    public Button RestartButton;
    public Button PlayAgainButton;

    [Space(10)]
    [Header("GameObjects")]
    public Transform HouseSlider;
    public BaseHandler CurrentSelectedBase = null;
    public GameObject SelectionPanel;
    public GameObject LevelSucessPanel;

}
