using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommodityView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public RunSkillView SkillView;
    public TMP_Text PriceText;
    public Button BuyButton;

    public event Action<Commodity> BuyEvent;
    public void ClearBuyEvent() => BuyEvent = null;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        SkillView.Configure(new IndexPath($"{_indexPath}.Skill"));

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(Buy);
    }

    public void Refresh()
    {
        Commodity commodity = DataManager.Get<Commodity>(_indexPath);

        bool isReveal = commodity != null;
        gameObject.SetActive(isReveal);
        if (!isReveal)
            return;

        SkillView.Refresh();
        PriceText.text = commodity.FinalPrice.ToString();
        BuyButton.interactable = commodity.Affordable();
    }

    private void Buy()
    {
        Commodity commodity = DataManager.Get<Commodity>(_indexPath);
        BuyEvent?.Invoke(commodity);
        RunCanvas.Instance.Refresh();
    }
}
