using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarterItemView : MonoBehaviour, IAddress
{
    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public RunSkillView PlayerSkillView;
    public Button ExchangeButton;
    public RunSkillView SkillView;

    public event Action<BarterItem> ExchangeEvent;
    public void ClearExchangeEvent() => ExchangeEvent = null;

    public void SetAddress(Address address)
    {
        _address = address;
        PlayerSkillView.SetAddress(_address.Append(".PlayerSkill"));
        SkillView.SetAddress(_address.Append(".Skill"));

        ExchangeButton.onClick.RemoveAllListeners();
        ExchangeButton.onClick.AddListener(Exchange);
    }

    public void Refresh()
    {
        BarterItem barterItem = Get<BarterItem>();

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
        BarterItem barterItem = Get<BarterItem>();
        ExchangeEvent?.Invoke(barterItem);
        RunCanvas.Instance.Refresh();
    }
}
