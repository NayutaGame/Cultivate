using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInventoryView : InventoryView<SkillView>, IDropHandler
{
    public ScrollRect ScrollRect;

    public Button[] DrawButtons;
    public Button[] SortButtons;

    public HoldButton LeftButton;
    public HoldButton RightButton;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        DrawButtons.Length.Do(i =>
        {
            JingJie jingJie = i;
            DrawButtons[i].onClick.AddListener(() => DrawJingJie(jingJie));
        });

        SortButtons.Length.Do(i =>
        {
            int comparisonId = i;
            SortButtons[i].onClick.AddListener(() => SortByComparisonId(comparisonId));
        });

        if (LeftButton != null)
            LeftButton.HoldAction = () =>
                ScrollRect.horizontalNormalizedPosition -= 2000f / ScrollRect.content.rect.width * Time.deltaTime;

        if (RightButton != null)
            RightButton.HoldAction = () =>
                ScrollRect.horizontalNormalizedPosition += 2000f / ScrollRect.content.rect.width * Time.deltaTime;
    }

    public void OnDrop(PointerEventData eventData)
    {
        IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        if (drag == null)
            return;

        IInteractable drop = GetComponent<IInteractable>();
        if (drag == drop)
            return;

        drag.GetDelegate()?.DragDrop(drag, drop);
    }

    private void DrawJingJie(JingJie jingJie)
    {
        RunManager.Instance.Battle.ForceDrawSkill(jingJie: jingJie);
        RunCanvas.Instance.Refresh();
    }

    private void SortByComparisonId(int i)
    {
        SkillInventory inventory = DataManager.Get<SkillInventory>(GetIndexPath());
        inventory.SortByComparisonId(i);
        RunCanvas.Instance.Refresh();
    }
}
