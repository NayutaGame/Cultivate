using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour, IIndexPath
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public Button CopyButton;
    public InventoryView<AbstractSkillView> EquippedInventoryView;

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
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());

        if (entity == null)
            return;

        NameText.text = entity.GetEntry()?.Name ?? "未命名";
        JingJieText.text = entity.GetJingJie().ToString();
        HPText.text = entity.GetHealth().ToString();
        DescriptionText.text = entity.GetEntry()?.Description ?? "这家伙很懒，什么都没有写";
        EquippedInventoryView.Refresh();
    }

    private void Copy()
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        GUIUtility.systemCopyBuffer = entity.ToJson();
    }
}
