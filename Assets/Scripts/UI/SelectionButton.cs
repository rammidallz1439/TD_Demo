using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vault;

public class SelectionButton : MonoBehaviour
{
    [SerializeField] private GameObject SelectionPanel;
    [SerializeField] private int Cost;
    [SerializeField] private GameObject NoCoinsText;
    [SerializeField] private TMP_Text CoinsText;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private LevelHandler Handler;

    private void Start()
    {
        CostText.text = Cost.ToString();
    }
    public void OnSelectionButtonClicked(GameObject turret)
    {
        if (Cost <= GlobalManager.Instance.TotalCoins)
        {
            GlobalManager.Instance.TotalCoins -= Cost;
            PlayerPrefs.SetInt(GameConstants.CoinsCount,GlobalManager.Instance.TotalCoins);
            CoinsText.text = GlobalManager.Instance.TotalCoins.ToString();
            EventManager.Instance.TriggerEvent(new SpawnTurretEvent(turret));
            SelectionPanel.SetActive(false);

        }
        else
        {
            NoCoinsText.SetActive(true);
            MEC.Timing.CallDelayed(1f, () => {
                NoCoinsText.SetActive(false);
                SelectionPanel.SetActive(false);
                Handler.CurrentSelectedBase.transform.DOMoveY(Handler.CurrentSelectedBase.InitialYPos, 0.5f);
                Handler.CurrentSelectedBase.transform.GetComponent<MeshRenderer>().material.color = Handler.CurrentSelectedBase.GetColor();

            });
        }
    }
}
