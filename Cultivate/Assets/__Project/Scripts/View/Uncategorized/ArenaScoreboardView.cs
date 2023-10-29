using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArenaScoreboardView : MonoBehaviour
{
    public Transform VLayoutContainer;
    private TMP_Text[] VLayoutTexts;

    public Transform HLayoutContainer;
    private TMP_Text[] HLayoutTexts;

    public Transform GridLayoutContainer;
    private TMP_Text[] GridLayoutTexts;
    private Image[] GridLayoutImages;

    public void Configure()
    {
        VLayoutTexts = new TMP_Text[VLayoutContainer.childCount];
        for (int i = 0; i < VLayoutContainer.childCount; i++)
            VLayoutTexts[i] = VLayoutContainer.GetChild(i).GetComponent<TMP_Text>();

        HLayoutTexts = new TMP_Text[HLayoutContainer.childCount];
        for (int i = 0; i < HLayoutContainer.childCount; i++)
            HLayoutTexts[i] = HLayoutContainer.GetChild(i).GetComponent<TMP_Text>();

        GridLayoutTexts = new TMP_Text[GridLayoutContainer.childCount];
        GridLayoutImages = new Image[GridLayoutContainer.childCount];
        for (int i = 0; i < GridLayoutContainer.childCount; i++)
        {
            Transform child = GridLayoutContainer.GetChild(i);
            GridLayoutTexts[i] = child.GetComponentInChildren<TMP_Text>();
            GridLayoutImages[i] = child.GetComponentInChildren<Image>();
            int index = i;
            child.GetComponentInChildren<Button>().onClick.AddListener(() => ShowReport(index));
        }
    }

    public void Refresh()
    {
        Arena arena = RunManager.Instance.Arena;
        for (int i = 0; i < arena.Count(); i++)
        {
            VLayoutTexts[i].text = i.ToString();
            HLayoutTexts[i].text = i.ToString();
        }

        for (int i = 0; i < GridLayoutTexts.Length; i++)
        {
            StageEnvironmentResult result = arena.Reports[i];
            if (result != null)
            {
                GridLayoutImages[i].color = result.HomeVictory ? Color.green : Color.red;
                GridLayoutTexts[i].text = $"{result.HomeLeftHp} : {result.AwayLeftHp}";
            }
        }
    }

    private void ShowReport(int i)
    {
        Arena arena = RunManager.Instance.Arena;
        arena.ShowReport(i);
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
