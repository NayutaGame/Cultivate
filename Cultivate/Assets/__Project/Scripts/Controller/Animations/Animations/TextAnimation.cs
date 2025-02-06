
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
        var spawnPos = _model.VFXTransform.position + 
                       new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
    
        GameObject gao = StageManager.Instance.FetchObject(GetPrefab(type));
        gao.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
    
        FloatTextVFX vfx = gao.GetComponent<FloatTextVFX>();
        TMP_Text text = vfx.Text;

        text.text = content;
        text.alpha = 1;
    
        gao.transform.localScale = Vector3.zero;
        text.transform.localScale = Vector3.one;
        text.transform.localPosition = Vector3.zero;
        text.transform.localRotation = Quaternion.identity;

        switch (type)
        {
            case TextEffectType.Buff:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                break;
            case TextEffectType.Debuff:
                text.color = new Color(0.9f, 0.2f, 0.8f);
                break;
            case TextEffectType.Mana:
                text.color = new Color(0.65f, 0.8f, 1f);
                break;
            case TextEffectType.LoseBuff:
                text.color = new Color(0.8f, 0.8f, 0.8f);
        
                DOTween.Sequence()
                    .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
                    .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
                    .SetAutoKill().Restart();
        
                DOTween.Sequence()
                    .Append(gao.transform.DOMoveY(spawnPos.y - 2, 1f).From(spawnPos.y + 1))
                    .SetAutoKill().Restart();

                DOTween.Sequence()
                    .Append(text.DOFade(0, 0.5f).SetDelay(0.5f))
                    .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
                    .SetAutoKill().Restart();
                return;
            case TextEffectType.NoDamage:
                text.color = new Color(0.8f, 0.8f, 0.8f);
                break;
            case TextEffectType.Damage:
                text.color = new Color(0.9f, 0.2f, 0.1f);
                
                DOTween.Sequence()
                    .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
                    .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
                    .SetAutoKill().Restart();
                
                DOTween.Sequence()
                    .Append(gao.transform.DOMoveY(spawnPos.y + 3 + Random.Range(-0.5f, 0.5f), 1f).SetEase(Ease.OutCubic))
                    .Join(gao.transform.DOMoveX(spawnPos.x + Random.Range(-1f, 1f), 1f))
                    .SetAutoKill().Restart();
                
                DOTween.Sequence()
                    .Append(text.DOFade(0, 0.5f).SetDelay(0.5f))
                    .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
                    .SetAutoKill().Restart();
                return;
            case TextEffectType.HighDamage:
                text.color = new Color(0.9f, 0.2f, 0.1f);
                
                DOTween.Sequence()
                    .Append(text.transform.DOShakeScale(1.5f, strength: 2f, vibrato: 20))
                    .Join(text.transform.DOShakePosition(1, 0.1f))
                    .Join(text.transform.DOShakeRotation(0.5f, 3f))
                    .SetAutoKill().Restart();
                
                DOTween.Sequence()
                    .Append(gao.transform.DOScale(4, 0.3f))
                    .Append(gao.transform.DOScale(3, 1.7f))
                    .SetAutoKill().Restart();
                
                DOTween.Sequence()
                    .Append(text.DOFade(0, 1.5f).SetDelay(0.5f))
                    .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
                    .SetAutoKill().Restart();
                return;
            case TextEffectType.LoseArmor:
                text.color = new Color(0.6f, 0.3f, 1f);
        
                DOTween.Sequence()
                    .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
                    .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
                    .SetAutoKill().Restart();
        
                DOTween.Sequence()
                    .Append(gao.transform.DOMoveY(spawnPos.y - 2, 1f).From(spawnPos.y + 1))
                    .SetAutoKill().Restart();

                DOTween.Sequence()
                    .Append(text.DOFade(0, 0.5f).SetDelay(0.5f))
                    .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
                    .SetAutoKill().Restart();
                return;
            case TextEffectType.GainArmor:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                break;
            case TextEffectType.Heal:
                text.color = new Color(0.2f, 0.9f, 0.3f);
                break;
            case TextEffectType.Guarded:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                
                text.alpha = 0;
                gao.transform.localScale = Vector3.one * 2.5f;
        
                DOTween.Sequence()
                    .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.InCubic))
                    .SetAutoKill().Restart();

                DOTween.Sequence()
                    .Append(text.DOFade(1, 0.7f)).SetEase(Ease.InCubic)
                    .Append(text.DOFade(0, 0.1f)).SetDelay(0.2f).SetEase(Ease.OutCubic)
                    .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
                    .SetAutoKill().Restart();
                return;
            case TextEffectType.Formation:
                text.color = new Color(0.9f, 0.8f, 0.1f);
                break;
        }
        
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
