using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour, IAddress, IInteractable
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public Button CopyButton;
    public ListView<SkillView> EquippedInventoryView;

    #region Interact

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate()
        => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        EquippedInventoryView.SetDelegate(InteractDelegate);
    }

    #endregion

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public void Configure(Address address)
    {
        _address = address;
        CopyButton.onClick.AddListener(Copy);
        EquippedInventoryView.Configure(GetAddress().Append($"Slots"));
    }

    public void Refresh()
    {
        IEntityModel entity = Get<IEntityModel>();

        if (entity == null)
            return;

        NameText.text = entity.GetEntry()?.Name ?? "未命名";
        JingJieText.text = entity.GetJingJie().ToString();
        HPText.text = entity.GetFinalHealth().ToString();
        DescriptionText.text = entity.GetEntry()?.Description ?? "这家伙很懒，什么都没有写";
        EquippedInventoryView.Refresh();
    }

    private void Copy()
    {
        IEntityModel entity = Get<IEntityModel>();
        GUIUtility.systemCopyBuffer = entity.ToJson();
    }
}
