
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public class RunChipView : ItemView,
    IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected Image _image;

    public TMP_Text LevelText;
    public TMP_Text ManacostText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text PowerText;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    protected void SetColorFromJingJie(JingJie jingJie)
    {
        _image.color = CanvasManager.Instance.JingJieColors[jingJie];
    }

    public override void Refresh()
    {
        base.Refresh();

        if (RunManager.Get<AcquiredRunChip>(GetIndexPath()) is { } acquiredRunChip)
        {
            gameObject.SetActive(true);

            // LevelText.text = acquiredRunChip.GetLevel().ToString();
            LevelText.text = "";
            int manaCost = acquiredRunChip.GetManaCost();
            ManacostText.text = manaCost == 0 ? "" : manaCost.ToString();
            NameText.text = acquiredRunChip.GetName();
            SetDescriptionText(acquiredRunChip.GetDescription());
            PowerText.text = acquiredRunChip.GetPowerString();

            SetColorFromJingJie(acquiredRunChip.GetJingJie());
        }
        else if (RunManager.Get<HeroChipSlot>(GetIndexPath()) is { } heroChipSlot)
        {
            bool reveal = heroChipSlot.IsReveal();

            gameObject.SetActive(reveal);
            if (!reveal) return;

            if(heroChipSlot.AcquiredRunChip == null)
            {
                LevelText.text = "";
                ManacostText.text = "";
                NameText.text = "空";
                SetDescriptionText("");
                PowerText.text = heroChipSlot.GetPowerString();
                SetColorFromJingJie(JingJie.LianQi);
                return;
            }
            else
            {
                // LevelText.text = heroChipSlot.GetLevel().ToString();
                LevelText.text = "";

                int manaCost = heroChipSlot.GetManaCost();
                ManacostText.text = manaCost == 0 ? "" : manaCost.ToString();
                bool manaShortage = heroChipSlot.IsManaShortage();
                ManacostText.color = manaShortage ? Color.red : Color.black;

                NameText.text = heroChipSlot.GetName();
                SetDescriptionText(heroChipSlot.GetDescription());
                PowerText.text = heroChipSlot.GetPowerString();

                SetColorFromJingJie(heroChipSlot.RunChip.JingJie);
            }
        }
        else if(RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            bool reveal = enemyChipSlot.IsReveal;

            gameObject.SetActive(reveal);
            if (!reveal) return;

            if(enemyChipSlot.Chip == null)
            {
                LevelText.text = "";
                ManacostText.text = "";
                NameText.text = "空";
                SetDescriptionText("");
                PowerText.text = enemyChipSlot.GetPowerString();
                SetColorFromJingJie(JingJie.LianQi);
                return;
            }
            else
            {
                // LevelText.text = enemyChipSlot.Chip.Level.ToString();
                LevelText.text = "";
                int manaCost = enemyChipSlot.GetManaCost();
                ManacostText.text = manaCost == 0 ? "" : manaCost.ToString();
                NameText.text = enemyChipSlot.Chip.GetName();
                SetDescriptionText(enemyChipSlot.GetDescription());
                PowerText.text = enemyChipSlot.GetPowerString();
                SetColorFromJingJie(enemyChipSlot.Chip.JingJie);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetDescriptionText(string s)
    {
        if (DescriptionText != null)
            DescriptionText.text = s;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            enemyChipSlot.TryIncreseJingJie();
            RunCanvas.Instance.Refresh();
            return;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            eventData.pointerDrag = null;

            RunCanvas.Instance.ChipPreview.Configure(null);
            RunCanvas.Instance.ChipPreview.Refresh();
            return;
        }

        RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        RunCanvas.Instance.GhostChip.Configure(GetIndexPath());
        RunCanvas.Instance.GhostChip.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.ChipPreview.Configure(null);
        RunCanvas.Instance.ChipPreview.Refresh();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

        RunCanvas.Instance.GhostChip.Configure(null);
        RunCanvas.Instance.GhostChip.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        RunCanvas.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.GhostChip.UpdateMousePos(eventData.position);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        if (RunManager.Get<AcquiredRunChip>(GetIndexPath()) is { } acquiredRunChip)
        {
            AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
            if (fromAcquired != null)
            {
                if (RunManager.Instance.AcquiredInventory.TryMerge(fromAcquired, acquiredRunChip)) return;
                if (RunManager.Instance.AcquiredInventory.Swap(fromAcquired, acquiredRunChip)) return;
                return;
            }

            HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
            if (fromHeroChipSlot != null)
            {
                if (fromHeroChipSlot.TryUnequip(acquiredRunChip)) return;
                return;
            }
        }
        else if (RunManager.Get<HeroChipSlot>(GetIndexPath()) is { } heroChipSlot)
        {
            AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
            if (fromAcquired != null)
            {
                if (heroChipSlot.TryEquip(fromAcquired)) return;
                return;
            }

            HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
            if (fromHeroChipSlot != null)
            {
                if (RunManager.Instance.Hero.HeroSlotInventory.Swap(fromHeroChipSlot, heroChipSlot)) return;
                return;
            }
        }
        else if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            RunChip fromArenaChip = RunManager.Get<RunChip>(drop.GetIndexPath());
            if (fromArenaChip != null)
            {
                if (enemyChipSlot.TryWrite(fromArenaChip)) return;
                return;
            }

            AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
            if (fromAcquired != null)
            {
                if (enemyChipSlot.TryWrite(fromAcquired)) return;
                return;
            }

            HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
            if (fromHeroChipSlot != null)
            {
                if (enemyChipSlot.TryWrite(fromHeroChipSlot)) return;
                return;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.ChipPreview.Configure(GetIndexPath());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.ChipPreview.Configure(null);
        RunCanvas.Instance.ChipPreview.Refresh();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.ChipPreview.UpdateMousePos(eventData.position);
        RunCanvas.Instance.ChipPreview.Refresh();
    }
}
