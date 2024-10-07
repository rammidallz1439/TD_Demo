using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

 
    public int TotalCoins;

    private void Awake()
    {
        Instance = this; 
        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        GenericEventsController.Instance.CheckIntData(GameConstants.CoinsCount, ref TotalCoins, GameConstants.InitialCoins);
    }

    public void AddCoins(int value)
    {
        TotalCoins += value;
        PlayerPrefs.SetInt(GameConstants.CoinsCount, TotalCoins);
    }
}
