using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelAndMoneyUI : MonoBehaviour

{
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Money;
    public MoneyManager MoneyValue;
    public PlayerLevelManager PlayerLevelManager;
    // Start is called before the first frame update
    private void Start()
    {
        Level.enabled = true;
        Money.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Money.text = "$:" + MoneyValue.money.ToString();
        Level.text = "XP:" + PlayerLevelManager.playerLevel.ToString();
    }
}
