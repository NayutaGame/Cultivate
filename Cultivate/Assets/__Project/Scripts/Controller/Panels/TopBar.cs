
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public ResourceView MingYuan;
    public ResourceView Gold;
    public ResourceView Health;
    
    public TMP_Text JingJieText;
    public PropagatePointer PropagateJingJieText;
    public RectTransform PropagateJingJieRT;

    public TMP_Text DifficultyText;
    public PropagatePointer PropagateDifficultyText;
    public RectTransform PropagateDifficultyRT;

    public Button MenuButton;

    private bool _hasAwoken;
    
    public virtual void Awake()
    {
        CheckAwake();
    }

    public void CheckAwake()
    {
        if (_hasAwoken)
            return;
        _hasAwoken = true;
        AwakeFunction();
    }
    
    public virtual void AwakeFunction()
    {
        MenuButton.onClick.RemoveAllListeners();
        MenuButton.onClick.AddListener(OpenMenu);
    }

    private void OnEnable()
    {
        MingYuan.Configure(1, RunManager.Instance.Environment.GetMingYuan, RunManager.Instance.Environment.GetMingYuan().GetMingYuanPenaltyText);
        Gold.Configure(1, RunManager.Instance.Environment.GetGold, () => "金钱");
        Health.Configure(1, RunManager.Instance.Environment.Home.GetHealthBounded, () => "气血上限\n战斗开始的气血");
        
        RunManager.Instance.Environment.GainMingYuanNeuron.Add(GainMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(LoseMingYuan);
        RunManager.Instance.Environment.GainGoldNeuron.Add(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(LoseGold);
        RunManager.Instance.Environment.GainHealthNeuron.Add(GainHealth);
        RunManager.Instance.Environment.LoseHealthNeuron.Add(LoseHealth);
        
        RunManager.Instance.Environment.JingJieChangedNeuron.Add(RefreshJingJieText);

        PropagateJingJieText._onPointerEnter += PointerEnterJingJieText;
        PropagateJingJieText._onPointerExit += PointerExitJingJieText;

        PropagateDifficultyText._onPointerEnter += PointerEnterDifficultyText;
        PropagateDifficultyText._onPointerExit += PointerExitDifficultyText;
        
        Refresh();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainMingYuanNeuron.Remove(GainMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(LoseMingYuan);
        RunManager.Instance.Environment.GainGoldNeuron.Remove(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Remove(LoseGold);
        RunManager.Instance.Environment.GainHealthNeuron.Remove(GainHealth);
        RunManager.Instance.Environment.LoseHealthNeuron.Remove(LoseHealth);
        
        RunManager.Instance.Environment.JingJieChangedNeuron.Remove(RefreshJingJieText);

        PropagateJingJieText._onPointerEnter -= PointerEnterJingJieText;
        PropagateJingJieText._onPointerExit -= PointerExitJingJieText;
        
        PropagateDifficultyText._onPointerEnter -= PointerEnterDifficultyText;
        PropagateDifficultyText._onPointerExit -= PointerExitDifficultyText;
    }

    private void GainMingYuan(int value)
    {
        MingYuan.Gain(new Vector2(Screen.width / 2, Screen.height / 2), value);
    }

    private void LoseMingYuan(int value)
    {
        MingYuan.LoseNoAnimation(value);
    }

    private void GainGold(int value)
    {
        Gold.Gain(new Vector2(Screen.width / 2, Screen.height / 2), value);
    }

    private void LoseGold(int value)
    {
        Gold.Lose(value);
    }

    private void GainHealth(int value)
    {
        Health.Gain(new Vector2(Screen.width / 2, Screen.height / 2), value);
    }

    private void LoseHealth(int value)
    {
        Health.Lose(value);
    }

    private void RefreshJingJieText(JingJieChangedDetails d)
    {
        JingJieText.text = $"{d.ToJingJie.ToString()}期";
    }

    public void Refresh()
    {
        MingYuan.Refresh();
        Gold.Refresh();
        Health.Refresh();
        
        DifficultyText.text = $"难度{RunManager.Instance.Environment.GetRunConfig().GetDifficulty()}";
        JingJieText.text = $"{RunManager.Instance.Environment.JingJie.ToString()}期";
    }

    private void OpenMenu()
    {
        AppManager.Instance.Push(AppStateMachine.MENU);
    }
    
    private void PointerEnterJingJieText(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerEnter(PropagateJingJieRT, d, RunManager.Instance.Environment.GetJingJieHintText());
    }

    private void PointerExitJingJieText(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerExit(d);
    }
    
    private void PointerEnterDifficultyText(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerEnter(PropagateDifficultyRT, d, RunManager.Instance.Environment.GetDifficultyHintText());
    }

    private void PointerExitDifficultyText(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerExit(d);
    }
}
