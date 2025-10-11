
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
        Vector3 spawnPos = _model.VFXTransform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
    
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
        float orient = _model.transform.right.x;

        switch (type)
        {
            case TextEffectType.Buff:
                SpawnBuffText(text, gao, spawnPos);
                return;
            case TextEffectType.Debuff:
                SpawnDebuffText(text, gao, spawnPos);
                return;
            case TextEffectType.Mana:
                SpawnManaText(text, gao, spawnPos);
                return;
            case TextEffectType.LoseBuff:
                SpawnLoseBuffText(text, gao, spawnPos);
                return;
            case TextEffectType.NoDamage:
                text.color = new Color(0.8f, 0.8f, 0.8f);
                break;
            case TextEffectType.Damage:
                SpawnDamageText(text, gao, spawnPos, orient);
                return;
            case TextEffectType.HighDamage:
                SpawnHighDamageText(text, gao, spawnPos, orient);
                return;
            case TextEffectType.LoseArmor:
                SpawnLoseArmorText(text, gao, spawnPos);
                return;
            case TextEffectType.GainArmor:
                SpawnGainArmorText(text, gao, spawnPos);
                return;
            case TextEffectType.Heal:
                text.color = new Color(0.2f, 0.9f, 0.3f);
                break;
            case TextEffectType.Guarded:
                SpawnGuardedText(text, gao, spawnPos, orient);
                return;
            case TextEffectType.Formation:
                SpawnFormationText(text, gao, spawnPos);
                return;
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

    private void SpawnGuardedText(TMP_Text text, GameObject gao, Vector3 spawnPos, float orient)
    {
        text.color = new Color(0.9f, 0.8f, 0.1f);
        
        gao.transform.localScale = Vector3.one * 2.5f;
        gao.transform.position = spawnPos + new Vector3(orient * 0.5f, 1.5f, 0f);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .AppendInterval(0.7f)
            .Append(text.DOFade(0, 0.1f)).SetDelay(0.2f).SetEase(Ease.OutCubic)
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Guarded), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnLoseBuffText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.8f, 0.8f, 0.8f);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y - 1, .7f).From(spawnPos.y + 1).SetEase(Ease.InQuad))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.LoseBuff), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnLoseArmorText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.6f, 0.3f, 1f);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y - 1, .7f).From(spawnPos.y + 1).SetEase(Ease.InQuad))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.LoseArmor), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnHighDamageText(TMP_Text text, GameObject gao, Vector3 spawnPos, float orient)
    {
        text.color = new Color(0.9f, 0.2f, 0.1f);
        
        gao.transform.localScale = Vector3.one * 3f;
        DOTween.Sequence()
            .Append(gao.transform.DOPunchScale(
                punch: Vector3.one * 2f,
                duration: .5f,
                vibrato: 20,
                elasticity: 0.5f))
            .Join(gao.transform.DOPunchRotation(
                punch: Vector3.one * 40f,
                duration: .3f,
                vibrato: 30,
                elasticity: 0.8f))
            .SetAutoKill().Restart();
        
        Vector2 gain = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 2f));
        DOTween.Sequence()
            .Append(gao.transform.DOMoveX(spawnPos.x + gain.x, .1f).SetEase(Ease.OutCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + gain.y, .1f).SetEase(Ease.OutCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(text.DOFade(0, .3f).SetDelay(1f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.HighDamage), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnDamageText(TMP_Text text, GameObject gao, Vector3 spawnPos, float orient)
    {
        text.color = new Color(0.9f, 0.2f, 0.1f);
                
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        Vector2 gain = new Vector2(Random.Range(1.5f, 2.5f), Random.Range(0.5f, 1.5f));
        DOTween.Sequence()
            .Append(gao.transform.DOMoveX(spawnPos.x - orient * gain.x, 1f).SetEase(Ease.InOutQuad))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + gain.y, .3f).SetEase(Ease.OutQuad))
            .Append(gao.transform.DOMoveY(spawnPos.y + gain.y - 2f, .7f).SetEase(Ease.InQuad))
            .SetAutoKill().Restart();
                
        DOTween.Sequence()
            .Append(text.DOFade(0, 2f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Damage), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnDebuffText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.76f, 0.17f, 0.72f);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        float yGain = Random.Range(-0.5f, 0.5f);
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 2f + yGain, .5f).SetEase(Ease.OutCubic))
            .Join(gao.transform.DOMoveX(spawnPos.x + Random.Range(-.5f, .5f), .5f))
            .Append(gao.transform.DOMoveY(spawnPos.y + 1.5f + yGain, .3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
                
        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Debuff), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnBuffText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.9f, 0.8f, 0.1f);
                
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        float yGain = Random.Range(-0.5f, 0.5f);
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 2f + yGain, .5f).SetEase(Ease.OutCubic))
            .Join(gao.transform.DOMoveX(spawnPos.x + Random.Range(-.5f, .5f), .5f))
            .Append(gao.transform.DOMoveY(spawnPos.y + 2.5f + yGain, .3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
                
        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Buff), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnGainArmorText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.9f, 0.8f, 0.1f);
                
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        float yGain = Random.Range(-0.5f, 0.5f);
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 2f + yGain, .5f).SetEase(Ease.OutCubic))
            .Join(gao.transform.DOMoveX(spawnPos.x + Random.Range(-.5f, .5f), .5f))
            .Append(gao.transform.DOMoveY(spawnPos.y + 2.5f + yGain, .3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
                
        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.GainArmor), gao))
            .SetAutoKill().Restart();
    }
    
    private void SpawnManaText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.65f, 0.8f, 1f);
                
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        float yGain = Random.Range(-0.5f, 0.5f);
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 2f + yGain, .5f).SetEase(Ease.OutCubic))
            .Join(gao.transform.DOMoveX(spawnPos.x + Random.Range(-.5f, .5f), .5f))
            .Append(gao.transform.DOMoveY(spawnPos.y + 2.5f + yGain, .3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
                
        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Mana), gao))
            .SetAutoKill().Restart();
    }
    
    private void SpawnFormationText(TMP_Text text, GameObject gao, Vector3 spawnPos)
    {
        text.color = new Color(0.9f, 0.8f, 0.1f);
                
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        float yGain2 = Random.Range(-0.5f, 0.5f);
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 2f + yGain2, .5f).SetEase(Ease.OutCubic))
            .Join(gao.transform.DOMoveX(spawnPos.x + Random.Range(-.5f, .5f), .5f))
            .Append(gao.transform.DOMoveY(spawnPos.y + 2.5f + yGain2, .3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
                
        DOTween.Sequence()
            .Append(text.DOFade(0, 0.5f).SetDelay(0.3f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Formation), gao))
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
            d.BuffEntry.GetName() == "灵气"
                ? TextEffectType.Mana
                : (d.BuffEntry.Friendly ? TextEffectType.Buff : TextEffectType.Debuff),
            $"{d.BuffEntry.GetName()}+{d.Stack}", false, d.Induced);

    public static TextAnimation FromLoseBuffDetails(LoseBuffDetails d)
        => new(d.Tgt.Model(), TextEffectType.LoseBuff, $"{d.BuffEntry.GetName()}-{d.Stack}", false, d.Induced);
    
    public static TextAnimation FromGainFormationDetails(GainFormationDetails d)
        => new(d.Owner.Model(), TextEffectType.Formation, d._formation.GetName(), false, d.Induced);
    
    public static TextAnimation FromDamageDetails(DamageDetails d)
        => new(d.Tgt.Model(), d.IsCritical ? TextEffectType.HighDamage : TextEffectType.Damage, d.Value.ToString("N0"), false, d.Induced);
    
    public static TextAnimation FromNoDamaged(DamageDetails d)
        => new(d.Tgt.Model(), TextEffectType.NoDamage, "无伤害", false, d.Induced);
    
    public static TextAnimation FromGainArmorDetails(GainArmorDetails d)
        => new(d.Tgt.Model(), TextEffectType.GainArmor, $"护甲+{d.Value}", false, d.Induced);
    
    public static TextAnimation FromLoseArmorDetails(LoseArmorDetails d)
        => new(d.Tgt.Model(), TextEffectType.LoseArmor, $"护甲-{d.Value}", false, d.Induced);
    
    public static TextAnimation FromGuardedDetails(GuardedDetails d)
        => new(d.Tgt.Model(), TextEffectType.Guarded, "完全防御", false, d.Induced);
    
    public static TextAnimation FromHealDetails(HealDetails d)
        => new(d.Tgt.Model(), TextEffectType.Heal, $"+{d.Value}", false, d.Induced);
}
