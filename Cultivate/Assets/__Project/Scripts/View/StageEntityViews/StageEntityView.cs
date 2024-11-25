
using Renge.PPB;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageEntityView : LegacySimpleView
{
    [SerializeField] private LegacyListView Formations;
    [SerializeField] private LegacyListView Buffs;
    [SerializeField] private ProceduralProgressBar HealthBar;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private Image ArmorIcon;
    [SerializeField] private TMP_Text ArmorText;

    [SerializeField] private PropagatePointer ArmorPropagatePointer;
    [SerializeField] private RectTransform ArmorRectTransform;

    public override void SetAddress(Address address)
    {
        if (Get<StageEntity>() is { } e1)
        {
            e1.HpChangedNeuron.Remove(HpChanged);
            e1.ArmorChangedNeuron.Remove(ArmorChanged);
        }
        
        base.SetAddress(address);
        
        if (Get<StageEntity>() is { } e2)
        {
            e2.HpChangedNeuron.Add(HpChanged);
            e2.ArmorChangedNeuron.Add(ArmorChanged);
        }

        Formations.SetAddress(GetAddress().Append(".Formations"));
        Formations.PointerEnterNeuron.Join(StageManager.Instance.Pause);
        Formations.PointerExitNeuron.Join(StageManager.Instance.Resume);

        Buffs.SetAddress(GetAddress().Append(".Buffs"));
        Buffs.PointerEnterNeuron.Join(StageManager.Instance.Pause);
        Buffs.PointerExitNeuron.Join(StageManager.Instance.Resume);

        HealthBar.PublicValidate();
    }

    private void OnEnable()
    {
        ArmorPropagatePointer._onPointerEnter = PointerEnter;
        ArmorPropagatePointer._onPointerExit = PointerExit;
    }

    private void OnDisable()
    {
        if (Get<StageEntity>() is { } e1)
        {
            e1.HpChangedNeuron.Remove(HpChanged);
            e1.ArmorChangedNeuron.Remove(ArmorChanged);
        }
        ArmorPropagatePointer._onPointerEnter -= PointerEnter;
        ArmorPropagatePointer._onPointerExit -= PointerExit;
    }

    private void PointerEnter(PointerEventData d)
    {
        if (d.dragging) return;
        StageManager.Instance.Pause();
        CanvasManager.Instance.TextHint.PointerEnter(ArmorRectTransform, d, GetArmorHint());
    }

    private void PointerExit(PointerEventData d)
    {
        if (d.dragging) return;
        StageManager.Instance.Resume();
        CanvasManager.Instance.TextHint.PointerExit(d);
    }

    private string GetArmorHint()
    {
        StageEntity entity = Get<StageEntity>();
        int armor = entity.Armor;
        
        if (armor > 0)
        {
            return "护甲\n可以抵消受到的攻击伤害";
        }
        else if (armor == 0)
        {
            return "没有护甲时，受到的攻击伤害不变";
        }
        else // armor < 0
        {
            return "破甲\n会加深下一次受到的攻击伤害";
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        Formations.Refresh();
        Buffs.Refresh();

        StageEntity entity = Get<StageEntity>();
        HpChanged(entity.Hp, entity.MaxHp);
        ArmorChanged(entity.Armor);
    }

    private void HpChanged(int hp, int maxHp)
    {
        HealthText.text = $"{hp}/{maxHp}";
        HealthBar.SegmentCount = maxHp / 100f;
        HealthBar.Value = hp / 100f;
    }

    private void ArmorChanged(int armor)
    {
        if (armor > 0)
        {
            ArmorIcon.gameObject.SetActive(true);
            ArmorText.font = CanvasManager.Instance.ArmorFontAsset;
            ArmorText.text = $"{armor}";
            ArmorIcon.sprite = Encyclopedia.SpriteCategory["ArmorIcon"].Sprite;
        }
        else if (armor == 0)
        {
            ArmorIcon.gameObject.SetActive(false);
        }
        else // armor < 0
        {
            ArmorIcon.gameObject.SetActive(true);
            ArmorText.font = CanvasManager.Instance.FragileFontAsset;
            ArmorText.text = $"{-armor}";
            ArmorIcon.sprite = Encyclopedia.SpriteCategory["FragileIcon"].Sprite;
        }
    }
}
