using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour, IIndexPath, IInteractable
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public Button CopyButton;
    public InventoryView<SkillView> EquippedInventoryView;

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

    private IndexPath _indexPath;
    public IndexPath GetIndexPath()
        => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        CopyButton.onClick.AddListener(Copy);
        EquippedInventoryView.Configure(new IndexPath($"{GetIndexPath()}.Slots"));
    }

    public void Refresh()
    {
        IEntityModel entity = DataManager.Get<IEntityModel>(GetIndexPath());

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
        IEntityModel entity = DataManager.Get<IEntityModel>(GetIndexPath());
        GUIUtility.systemCopyBuffer = entity.ToJson();
    }
}
