using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Vault;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public float Health;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Canvas _worldCanvas;
    [SerializeField] private int _dropValue;
    [SerializeField] private GameObject _Coin;
    public float AttackPower;
    public int CurrentPoint;
    private void Start()
    {
        _healthBar.maxValue = Health;
        _healthBar.value = Health;
        _worldCanvas.worldCamera = Camera.main; 
    }
    private void Update()
    {
        MonoHelper.Instance.FaceCamera(Camera.main, _healthBar.transform);

        EventManager.Instance.TriggerEvent(new EnemyMovementEvent(this));
    }

    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
            _healthBar.value = Health;
        }

        if (Health <= 0)
        {
            DropCoins();
            Destroy(gameObject);
        }
    }

    void DropCoins()
    {
        GameObject coin = null;
        int totalCoinsToDrop = 0;
        float radius = 1.5f; 

        if (_dropValue <= 10)
        {
            totalCoinsToDrop = GameConstants.SmallDropValue;
        }
        else if (_dropValue <= 20)
        {
            totalCoinsToDrop = GameConstants.MediumDropValue;
        }
        else if (_dropValue > 20)
        {
            totalCoinsToDrop = GameConstants.LargeDropValue;
        }

        for (int i = 0; i < totalCoinsToDrop; i++)
        {
            float angle = i * Mathf.PI * 2 / totalCoinsToDrop;

            Vector3 spawnPosition = new Vector3(
                transform.position.x + Mathf.Cos(angle) * radius,
                transform.position.y,  
                transform.position.z + Mathf.Sin(angle) * radius
            );

           
            coin = Instantiate(_Coin, spawnPosition, Quaternion.identity);
        }

        GlobalManager.Instance.AddCoins(_dropValue);
    }



}
