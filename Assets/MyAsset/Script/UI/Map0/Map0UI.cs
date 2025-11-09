using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapUI : LoadComponentMonoBehavior
{
    [SerializeField] private Transform healthPoints;
    [SerializeField] private PlayerLevel0 player;
    [SerializeField] private List<GameObject> hpIcons = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI guideText;


    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (hpIcons.Count == 0)
        {
            for (int i = 0; i < healthPoints.childCount; i++)
            {
                hpIcons.Add(healthPoints.GetChild(i).gameObject);
            }
        }
        if (!guideText) guideText = transform.Find("GuideText").GetComponent<TextMeshProUGUI>();
        if(!player) player = FindObjectOfType<PlayerLevel0>();
    }
    private void OnEnable()
    {
        player.OnPlayerLostHealth += UpdateHealthUI;
        player.OnGoNextLevel += UpdateNextLevelUI;
    }
    private void OnDisable()
    {
        player.OnPlayerLostHealth -= UpdateHealthUI;
        player.OnGoNextLevel -= UpdateNextLevelUI;
    }
    private void UpdateHealthUI(PlayerAbstract player)
    {
        for (int i = 0; i < hpIcons.Count; i++)
        {
            if (i < player.HP)
            {
                hpIcons[i].SetActive(true);
            }
            else
            {
                hpIcons[i].SetActive(false);
            }
        }
    }
    private void UpdateNextLevelUI(PlayerAbstract player)
    {
        guideText.gameObject.SetActive(false);
    }

}
