
using Renge.PPB;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageEntityView : SimpleView
{
    [SerializeField] private ListView Formations;
    [SerializeField] private ListView Buffs;
    [SerializeField] private ProceduralProgressBar HealthBar;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private Image ArmorIcon;
    [SerializeField] private TMP_Text ArmorText;

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
    }

    private void OnDisable()
    {
        if (Get<StageEntity>() is { } e1)
        {
            e1.HpChangedNeuron.Remove(HpChanged);
            e1.ArmorChangedNeuron.Remove(ArmorChanged);
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
