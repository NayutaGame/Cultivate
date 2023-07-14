using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarterItemView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public RunSkillView PlayerSkillView;
    public Button ExchangeButton;
    public RunSkillView SkillView;

    public event Action<BarterItem> ExchangeEvent;
    public void ClearExchangeEvent() => ExchangeEvent = null;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        PlayerSkillView.Configure(new IndexPath($"{_indexPath}.PlayerSkill"));
        SkillView.Configure(new IndexPath($"{_indexPath}.Skill"));

        ExchangeButton.onClick.RemoveAllListeners();
        ExchangeButton.onClick.AddListener(Exchange);
    }

    public void Refresh()
    {
        BarterItem barterItem = DataManager.Get<BarterItem>(_indexPath);

        bool isReveal = barterItem != null;
        gameObject.SetActive(isReveal);
        if (!isReveal)
            return;

        PlayerSkillView.Refresh();
        SkillView.Refresh();
        ExchangeButton.interactable = barterItem.Affordable();
    }

    private void Exchange()
    {
        BarterItem barterItem = DataManager.Get<BarterItem>(_indexPath);
        ExchangeEvent?.Invoke(barterItem);
        RunCanvas.Instance.Refresh();
    }
}
