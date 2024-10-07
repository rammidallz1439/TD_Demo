using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseHandler : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private float _health;
    [SerializeField] private GameObject _gameOverPanel;

    private void Start()
    {
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag is "Enemy")
        {
            TakeDamage(other.transform.GetComponent<Enemy>().AttackPower);
            Destroy(other.gameObject);
        }
    }

    void TakeDamage(float damage)
    {
        if(_health > 0)
        {
            _health -= damage;
            _healthBar.value = _health;
        }

        if(_health <= 0)
        {
            _gameOverPanel.SetActive(true);
        }
    }
}
