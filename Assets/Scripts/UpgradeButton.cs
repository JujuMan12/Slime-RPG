using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public delegate void UpgradeAction(CharacterBehavior.CharStat stat, int cost);
    public static event UpgradeAction OnUpgrade;

    private int currentCost;

    [Header("Upgrade")]
    [SerializeField] private Button button;
    [SerializeField] private CharacterBehavior.CharStat stat;
    [SerializeField] private TMPro.TextMeshProUGUI statLevel;
    [SerializeField] private TMPro.TextMeshProUGUI statCost;
    [SerializeField] private TMPro.TextMeshProUGUI statValue;
    [SerializeField] private TMPro.TextMeshProUGUI statBonus;
    [SerializeField] private int goldPerLevel = 10;

    private void OnEnable()
    {
        SlimeController.OnStatUpdate += UpdateStat;
        SlimeController.OnGoldUpdate += UpdateCost;
    }

    private void OnDisable()
    {
        SlimeController.OnStatUpdate -= UpdateStat;
        SlimeController.OnGoldUpdate -= UpdateCost;
    }

    public void UpgradeStat()
    {
        if (OnUpgrade != null) OnUpgrade(stat, currentCost);
    }

    private void UpdateStat(CharacterBehavior.CharStat updatedStat, int level, float bonus, float value)
    {
        if (updatedStat != stat)
        {
            return;
        }

        currentCost = (level + 1) * goldPerLevel;

        statLevel.text = $"Lvl {level}";
        statCost.text = $"${currentCost}";
        statValue.text = $"{value}";
        statBonus.text = $"+{bonus}";
    }

    private void UpdateCost(int gold)
    {
        button.interactable = gold >= currentCost;
    }
}
