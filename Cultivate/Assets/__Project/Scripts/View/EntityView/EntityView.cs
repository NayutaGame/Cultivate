using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : ItemView, IInteractable
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public Button CopyButton;
    public ListView EquippedInventoryView; // SkillView

    #region Interact

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        EquippedInventoryView.SetDelegate(InteractDelegate);
    }

    #endregion

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        CopyButton.onClick.AddListener(Copy);
        EquippedInventoryView.SetAddress(GetAddress().Append($"Slots"));
    }

    public override void Refresh()
    {
        base.Refresh();
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
