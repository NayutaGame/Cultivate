
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimation : Animation
{
    private IStageModel _model;
    private TextEffectType _type;
    private string _content;

    public TextAnimation(
        IStageModel model,
        TextEffectType type,
        string content,
        bool isAwait,
        bool induced)
        : base(isAwait, induced)
    {
        _model = model;
        _type = type;
        _content = content;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
        => SpawnText(_type, _content);
    
    private void SpawnText(TextEffectType type, string content)
    {
        var spawnPos = _model.transform.position + 
                       new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
    
        GameObject gao = StageManager.Instance.FetchObject(GetPrefab(type));
        gao.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
    
        FloatTextVFX vfx = gao.GetComponent<FloatTextVFX>();
        TMP_Text text = vfx.Text;

        text.text = content;
        text.alpha = 1;

        switch (type)
        {
            case TextEffectType.Buff:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                text.outlineColor = new Color(0.5f, 0.4f, 0.1f);
                break;
            case TextEffectType.Debuff:
                text.color = new Color(0.9f, 0.2f, 0.8f);
                text.outlineColor = new Color(0.5f, 0.1f, 0.4f);
                break;
            case TextEffectType.Mana:
                text.color = new Color(0.3f, 0.6f, 1f);
                text.outlineColor = new Color(0.1f, 0.3f, 0.6f);
                break;
            case TextEffectType.LoseBuff:
                text.color = new Color(0.8f, 0.8f, 0.8f);
                text.outlineColor = new Color(0.2f, 0.2f, 0.2f);
                break;
            case TextEffectType.NoDamage:
                text.color = new Color(0.8f, 0.8f, 0.8f);
                text.outlineColor = new Color(0.2f, 0.2f, 0.2f);
                break;
            case TextEffectType.Damage:
                text.color = new Color(0.9f, 0.2f, 0.1f);
                text.outlineColor = new Color(0.4f, 0.1f, 0.05f);
                // config.enableShake = true;
                break;
            case TextEffectType.HighDamage:
                text.color = new Color(0.9f, 0.2f, 0.1f);
                text.outlineColor = new Color(0.4f, 0.1f, 0.05f);
                // config.enableShake = true;
                break;
            case TextEffectType.LoseArmor:
                text.color = new Color(0.6f, 0.3f, 1f);
                text.outlineColor = new Color(0.3f, 0.1f, 0.6f);
                break;
            case TextEffectType.GainArmor:
                text.color = new Color(1f, 0.5f, 0f);
                text.outlineColor = new Color(0.5f, 0.2f, 0f);
                break;
            case TextEffectType.Heal:
                text.color = new Color(0.2f, 0.9f, 0.3f);
                text.outlineColor = new Color(0.1f, 0.4f, 0.1f);
                break;
            case TextEffectType.Guarded:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                text.outlineColor = new Color(0.5f, 0.4f, 0.1f);
                break;
            case TextEffectType.Formation:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                text.outlineColor = new Color(0.5f, 0.4f, 0.1f);
                break;
        }
    
        gao.transform.localScale = Vector3.zero;
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 3, 1f))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.5f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
            .SetAutoKill().Restart();
    }
    
    private GameObject GetPrefab(TextEffectType type)
    {
        return StageManager.Instance.FloatTextVFXPrefab;
    }
    
// #if UNITY_EDITOR
//     [CustomEditor(typeof(DamageTextSystem))]
//     public class DamageTextSystemEditor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//         
//             if(GUILayout.Button("Test Damage"))
//             {
//                 (target as DamageTextSystem).SpawnTestText(TextEffectType.Damage);
//             }
//         
//             if(GUILayout.Button("Test Heal"))
//             {
//                 (target as DamageTextSystem).SpawnTestText(TextEffectType.Heal);
//             }
//         }
//     }
// #endif

    public static TextAnimation FromGainBuffDetails(GainBuffDetails d)
        => new(d.Tgt.Model(),
            d._buffEntry.GetName() == "灵气"
                ? TextEffectType.Mana
                : (d._buffEntry.Friendly ? TextEffectType.Buff : TextEffectType.Debuff),
            $"{d._buffEntry.GetName()} +{d._stack}", false, d.Induced);

    public static TextAnimation FromLoseBuffDetails(LoseBuffDetails d)
        => new(d.Tgt.Model(), TextEffectType.LoseBuff, $"{d._buffEntry.GetName()} -{d._stack}", false, d.Induced);
    
    public static TextAnimation FromGainFormationDetails(GainFormationDetails d)
        => new(d.Owner.Model(), TextEffectType.Formation, d._formation.GetName(), false, d.Induced);
    
    public static TextAnimation FromDamageDetails(DamageDetails d)
        => new(d.Tgt.Model(), TextEffectType.Damage, d.Value.ToString("N0"), false, d.Induced);
    
    public static TextAnimation FromNoDamaged(DamageDetails d)
        => new(d.Tgt.Model(), TextEffectType.NoDamage, "无伤害", false, d.Induced);
    
    public static TextAnimation FromGainArmorDetails(GainArmorDetails d)
        => new(d.Tgt.Model(), TextEffectType.GainArmor, $"护甲 +{d.Value}", false, d.Induced);
    
    public static TextAnimation FromGuardedDetails(GuardedDetails d)
        => new(d.Tgt.Model(), TextEffectType.Guarded, "完全防御", false, d.Induced);
    
    public static TextAnimation FromHealDetails(HealDetails d)
        => new(d.Tgt.Model(), TextEffectType.Heal, $"+{d.Value}", false, d.Induced);
}
