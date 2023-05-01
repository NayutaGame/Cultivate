
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
    public TMP_Text SkillTypeText;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public override void Refresh()
    {
        base.Refresh();

        LevelText.text = "";

        if (RunManager.Get<AcquiredRunChip>(GetIndexPath()) is { } acquiredRunChip)
        {
            gameObject.SetActive(true);

            ManacostText.text = acquiredRunChip.GetManaCostString();
            NameText.text = acquiredRunChip.GetName();
            SetDescriptionText(acquiredRunChip.GetDescription());
            PowerText.text = acquiredRunChip.GetPowerString();
            SetSkillTypeText((acquiredRunChip.Chip._entry as WaiGongEntry).SkillTypeCollection);

            SetColorFromJingJie(acquiredRunChip.GetJingJie());

            _image.sprite = acquiredRunChip.Chip._entry.CardFace;
        }
        else if (RunManager.Get<HeroChipSlot>(GetIndexPath()) is { } heroChipSlot)
        {
            bool reveal = heroChipSlot.IsReveal();

            gameObject.SetActive(reveal);
            if (!reveal) return;

            if(heroChipSlot.AcquiredRunChip == null)
            {
                ManacostText.text = "";
                NameText.text = "空";
                SetDescriptionText("");
                PowerText.text = heroChipSlot.GetPowerString();
                SetColorFromJingJie(JingJie.LianQi);
                _image.sprite = null;
                SetSkillTypeText(null);
                return;
            }
            else
            {
                ManacostText.text = heroChipSlot.GetManaCostString();
                bool manaShortage = heroChipSlot.IsManaShortage();
                ManacostText.color = manaShortage ? Color.red : Color.black;

                NameText.text = heroChipSlot.GetName();
                SetDescriptionText(heroChipSlot.GetDescription());
                PowerText.text = heroChipSlot.GetPowerString();
                SetSkillTypeText((heroChipSlot.RunChip._entry as WaiGongEntry).SkillTypeCollection);

                SetColorFromJingJie(heroChipSlot.RunChip.JingJie);
                _image.sprite = heroChipSlot.RunChip._entry.CardFace;
            }
        }
        else if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            bool reveal = enemyChipSlot.IsReveal;

            gameObject.SetActive(reveal);
            if (!reveal) return;

            if(enemyChipSlot.Chip == null)
            {
                ManacostText.text = "";
                NameText.text = "空";
                SetDescriptionText("");
                PowerText.text = enemyChipSlot.GetPowerString();
                SetColorFromJingJie(JingJie.LianQi);
                _image.sprite = null;
                SetSkillTypeText(null);
                return;
            }
            else
            {
                ManacostText.text = enemyChipSlot.GetManaCostString();
                NameText.text = enemyChipSlot.Chip.GetName();
                SetDescriptionText(enemyChipSlot.GetDescription());
                PowerText.text = enemyChipSlot.GetPowerString();
                SetSkillTypeText((enemyChipSlot.Chip._entry as WaiGongEntry).SkillTypeCollection);
                SetColorFromJingJie(enemyChipSlot.Chip.JingJie);
                _image.sprite = enemyChipSlot.Chip._entry.CardFace;
            }
        }
        else if(RunManager.Get<RunChip>(GetIndexPath()) is { } runChip)
        {
            gameObject.SetActive(true);

            ManacostText.text = runChip.GetManaCostString();
            NameText.text = runChip.GetName();
            SetDescriptionText(runChip.GetDescription());
            PowerText.text = "";
            SetSkillTypeText((runChip._entry as WaiGongEntry).SkillTypeCollection);
            SetColorFromJingJie(runChip.JingJie);
            _image.sprite = runChip._entry.CardFace;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected void SetDescriptionText(string s)
    {
        if (DescriptionText != null)
            DescriptionText.text = s;
    }

    protected void SetSkillTypeText(SkillTypeCollection skillTypeCollection)
    {
        if (SkillTypeText == null)
            return;

        SkillTypeText.text = skillTypeCollection?.ToString();
    }

    protected void SetColorFromJingJie(JingJie jingJie)
    {
        _image.color = CanvasManager.Instance.JingJieColors[jingJie];
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
                // if (fromHeroChipSlot.TryUnequip(acquiredRunChip)) return;
                if (fromHeroChipSlot.TryUnequip()) return;
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
