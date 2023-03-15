using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI goldCount;

    private void OnEnable()
    {
        SlimeController.OnGoldUpdate += UpdateGoldCount;
    }

    private void OnDisable()
    {
        SlimeController.OnGoldUpdate -= UpdateGoldCount;
    }

    private void UpdateGoldCount(int gold)
    {
        goldCount.text = $"${gold}";
    }
}
