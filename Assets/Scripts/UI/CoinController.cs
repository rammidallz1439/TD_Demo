using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vault.EventManager.Instance.TriggerEvent(new CoinDobberAnimation(gameObject));
    }


}
