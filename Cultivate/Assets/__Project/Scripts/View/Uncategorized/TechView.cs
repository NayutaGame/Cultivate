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

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _image = GetComponent<Image>();
        SetDoneButton.onClick.AddListener(TrySetDone);
    }

    public override void Refresh()
    {
        base.Refresh();
        RunTech runTech = Get<RunTech>();

        NameText.text = runTech.GetName();

        if (runTech.HasEureka || runTech.State == RunTech.RunTechState.Done)
        {
            EurekaText.text = "";
        }
        else
        {
            EurekaText.text = runTech.GetEurekaString();
        }

        switch (runTech.State)
        {
            case RunTech.RunTechState.Done:
                _image.color = RunCanvas.Instance.TechColorGreen;
                RewardText.text = "";
                break;
            case RunTech.RunTechState.Current:
                _image.color = RunCanvas.Instance.TechColorYellow;
                RewardText.text = runTech.GetRewardsString();
                break;
            case RunTech.RunTechState.Locked:
                _image.color = RunCanvas.Instance.TechColorRed;
                RewardText.text = runTech.GetRewardsString();
                break;
        }

        bool isCurrent = runTech.State == RunTech.RunTechState.Current;
        SetDoneButton.gameObject.SetActive(isCurrent);
        if (!isCurrent) return;

        CostText.text = runTech.GetCost().ToString();
        CostText.color = RunManager.Instance.Environment.CanAffordTech(GetAddress()) ? Color.black : Color.red;

        if (runTech.HasEureka)
        {
            DiscountText.text = "已打折";
        }
        else
        {
            DiscountText.text = "";
        }
    }

    private void TrySetDone()
    {
        RunManager.Instance.Environment.TrySetDoneTech(GetAddress());
        RunCanvas.Instance.Refresh();
    }
}
