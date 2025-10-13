
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
        
        SpriteRenderer sr = vfx.Icon;
        int index = (int)type;
        sr.sprite = StageManager.Instance.FloatTextIcons[index];

        text.text = content;
        text.alpha = 1;
    
        gao.transform.localScale = Vector3.zero;
        float orient = _model.transform.right.x;
        
        text.fontSize = 5f;

        switch (type)
        {
            case TextEffectType.Buff:
                SpawnBuffText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.Debuff:
                SpawnDebuffText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.Mana:
                SpawnManaText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.LoseBuff:
                SpawnLoseBuffText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.NoDamage:
                SpawnNoDamageText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.Damage:
                SpawnDamageText(gao, sr, text, spawnPos, orient);
                return;
            case TextEffectType.HighDamage:
                SpawnHighDamageText(gao, sr, text, spawnPos, orient);
                return;
            case TextEffectType.LoseArmor:
                SpawnLoseArmorText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.GainArmor:
                SpawnGainArmorText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.Heal:
                SpawnHealText(gao, sr, text, spawnPos);
                return;
            case TextEffectType.Guarded:
                SpawnGuardedText(gao, sr, text, spawnPos, orient);
                return;
            case TextEffectType.Formation:
                SpawnFormationText(gao, sr, text, spawnPos);
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
            .AppendInterval(0.5f)
            .Append(text.DOFade(0, 0.5f))
            .Join(sr.DOFade(0, 0.5f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(type), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnBuffText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.965f, 0.898f, 0.445f, 1f);
        sr.color = new Color(1f, 0.823f, 0.411f, 1f);
                
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
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .Join(sr.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Buff), gao))
            .SetAutoKill().Restart();
    }
    
    private void SpawnDebuffText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.886f, 0.528f, 0.808f, 1f);
        sr.color = new Color(0.799f, 0.385f, 0.936f, 1f);
        
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
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .Join(sr.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Debuff), gao))
            .SetAutoKill().Restart();
    }
    
    private void SpawnManaText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.454f, 0.687f, 1f, 1f);
        sr.color = new Color(1, 1, 1, 1);
                
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
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .Join(sr.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Mana), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnLoseBuffText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.661f, 0.618f, 0.752f, 1f);
        sr.color = new Color(0.640f, 0.559f, 0.73f, 1f);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y - 1, .7f).From(spawnPos.y + 1).SetEase(Ease.InQuad))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f))
            .Join(sr.DOFade(0, 0.5f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.LoseBuff), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnNoDamageText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        sr.color = new Color(1, 1, 1, 1);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 3, 1f))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(text.DOFade(0, 0.5f))
            .Join(sr.DOFade(0, 0.5f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.NoDamage), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnDamageText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos, float orient)
    {
        text.color = new Color(0.9f, 0.2f, 0.1f, 1f);
        sr.color = new Color(1, 1, 1, 1);
        
        text.fontSize = 6.5f;
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
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
            .Join(sr.DOFade(0, 2f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Damage), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnHighDamageText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos, float orient)
    {
        text.color = new Color(0.902f, 0.102f, 0.181f, 1f);
        sr.color = new Color(1f, 0.936f, 0.468f, 1f);
        
        gao.transform.localScale = Vector3.one * 4f;
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
            .AppendInterval(1f)
            .Append(text.DOFade(0, .3f))
            .Join(sr.DOFade(0, .3f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.HighDamage), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnLoseArmorText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.765f, 0.529f, 0.957f, 1f);
        sr.color = new Color(1, 1, 1, 1);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y - 1, .7f).From(spawnPos.y + 1).SetEase(Ease.InQuad))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f))
            .Join(sr.DOFade(0, 0.5f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.LoseArmor), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnGainArmorText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.902f, 0.853f, 0.518f, 1f);
        sr.color = new Color(1, 1, 1, 1);
                
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
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .Join(sr.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.GainArmor), gao))
            .SetAutoKill().Restart();
    }

    private void SpawnHealText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.474f, 0.902f, 0.533f, 1f);
        sr.color = new Color(1, 1, 1, 1);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(.5f, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
        
        DOTween.Sequence()
            .Append(gao.transform.DOMoveY(spawnPos.y + 3, 1f))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(text.DOFade(0, 0.5f))
            .Join(sr.DOFade(0, 0.5f))
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Heal), gao))
            .SetAutoKill().Restart();
    }
    
    private void SpawnGuardedText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos, float orient)
    {
        text.color = new Color(0.918f, 0.837f, 1f, 1f);
        sr.color = new Color(1, 1, 1, 1);
        
        gao.transform.localScale = Vector3.one * 2.5f;
        gao.transform.position = spawnPos + new Vector3(orient * 0.5f, 1.5f, 0f);
        
        DOTween.Sequence()
            .Append(gao.transform.DOScale(2, 0.3f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();

        DOTween.Sequence()
            .AppendInterval(0.9f)
            .Append(text.DOFade(0, 0.1f)).SetEase(Ease.OutCubic)
            .Join(sr.DOFade(0, 0.1f)).SetEase(Ease.OutCubic)
            .OnComplete(() => StageManager.Instance.ReturnObject(GetPrefab(TextEffectType.Guarded), gao))
            .SetAutoKill().Restart();
    }
    
    private void SpawnFormationText(GameObject gao, SpriteRenderer sr, TMP_Text text, Vector3 spawnPos)
    {
        text.color = new Color(0.902f, 0.850f, 0.492f, 1f);
        sr.color = new Color(1f, 0.842f, 0.553f, 1f);
                
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
            .AppendInterval(0.3f)
            .Append(text.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
            .Join(sr.DOFade(0, 0.5f).SetEase(Ease.InOutQuart))
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
        => new(d.Tgt.Model(), TextEffectType.GainArmor, $"+{d.Value}", false, d.Induced);
    
    public static TextAnimation FromLoseArmorDetails(LoseArmorDetails d)
        => new(d.Tgt.Model(), TextEffectType.LoseArmor, $"-{d.Value}", false, d.Induced);
    
    public static TextAnimation FromGuardedDetails(GuardedDetails d)
        => new(d.Tgt.Model(), TextEffectType.Guarded, "完全防御", false, d.Induced);
    
    public static TextAnimation FromHealDetails(HealDetails d)
        => new(d.Tgt.Model(), TextEffectType.Heal, $"+{d.Value}", false, d.Induced);
}
