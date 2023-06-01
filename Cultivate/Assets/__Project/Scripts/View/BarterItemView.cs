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

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        PlayerSkillView.Configure(new IndexPath($"{_indexPath}.PlayerSkill"));
        SkillView.Configure(new IndexPath($"{_indexPath}.Skill"));

        ExchangeButton.onClick.AddListener(Exchange);
    }

    public void Refresh()
    {
        BarterItem barterItem = RunManager.Get<BarterItem>(_indexPath);
        if (barterItem == null)
        {
            gameObject.SetActive(false);
            return;
        }

        PlayerSkillView.Refresh();
        SkillView.Refresh();
        ExchangeButton.interactable = barterItem.Affordable();
    }

    private void Exchange()
    {
        BarterItem barterItem = RunManager.Get<BarterItem>(_indexPath);
        ExchangeEvent?.Invoke(barterItem);
        RunCanvas.Instance.Refresh();
    }
}
