using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechView : ItemView
{
    private Image _image;

    public TMP_Text NameText;
    public TMP_Text RewardText;
    public TMP_Text EurekaText;
    public Button SetDoneButton;
    public TMP_Text CostText;
    public TMP_Text DiscountText;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
        _image = GetComponent<Image>();
        SetDoneButton.onClick.AddListener(TrySetDone);
    }

    public override void Refresh()
    {
        base.Refresh();

        RunTech runTech = RunManager.Get<RunTech>(GetIndexPath());

        NameText.text = runTech.GetName();
        RewardText.text = runTech.GetRewardString();
        EurekaText.text = runTech.GetEurekaString();

        switch (runTech.State)
        {
            case RunTech.RunTechState.Done:
                _image.color = RunCanvas.Instance.GreenColor;
                break;
            case RunTech.RunTechState.Current:
                _image.color = RunCanvas.Instance.YellowColor;
                break;
            case RunTech.RunTechState.Locked:
                _image.color = RunCanvas.Instance.RedColor;
                break;
        }

        bool isCurrent = runTech.State == RunTech.RunTechState.Current;
        SetDoneButton.gameObject.SetActive(isCurrent);
        if (!isCurrent) return;

        CostText.text = runTech.GetCost().ToString();
        CostText.color = RunManager.Instance.CanAffordTech(GetIndexPath()) ? Color.black : Color.red;
        DiscountText.text = runTech.HasEureka ? "已打折" : "";
    }

    private void TrySetDone()
    {
        RunManager.Instance.TrySetDoneTech(GetIndexPath());
        RunCanvas.Instance.Refresh();
    }
}
