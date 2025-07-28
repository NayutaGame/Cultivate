
using System;
using Renge.PPB;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageEntityView : XView
{
    [SerializeField] public ListView Formations;
    [SerializeField] public ListView Buffs;
    [SerializeField] private ProceduralProgressBar HealthBar;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private Image ArmorIcon;
    [SerializeField] private TMP_Text ArmorText;
    [SerializeField] private XView ArmorView;

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

        Buffs.SetAddress(GetAddress().Append(".Buffs"));

        HealthBar.PublicValidate();
        
        ArmorView.SetAddress(GetAddress().Append(".ArmorDescription"));
    }

    private void OnEnable()
    {
        Formations.PointerEnterNeuron.Join(StageManager.SetHoverToTrue);
        Formations.PointerExitNeuron.Join(StageManager.SetHoverToFalse);
        Buffs.PointerEnterNeuron.Join(StageManager.SetHoverToTrue);
        Buffs.PointerExitNeuron.Join(StageManager.SetHoverToFalse);
    }

    private void OnDisable()
    {
        if (Get<StageEntity>() is { } e1)
        {
            e1.HpChangedNeuron.Remove(HpChanged);
            e1.ArmorChangedNeuron.Remove(ArmorChanged);
        }
        
        Formations.PointerEnterNeuron.Remove(StageManager.SetHoverToTrue);
        Formations.PointerExitNeuron.Remove(StageManager.SetHoverToFalse);
        Buffs.PointerEnterNeuron.Remove(StageManager.SetHoverToTrue);
        Buffs.PointerExitNeuron.Remove(StageManager.SetHoverToFalse);
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
