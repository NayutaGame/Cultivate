
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public ResourceView MingYuan;
    public ResourceView Gold;
    public ResourceView Health;

    public XView CycleIcon;
    
    public TMP_Text JingJieText;
    public XView JingJieAnnotationProvider;

    public TMP_Text DifficultyText;
    public XView DifficultyAnnotationProvider;

    public Button MenuButton;
    public PropagatePointerEnter MenuButtonPropagatePointerEnter;

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
        MenuButton.onClick.AddListener(AudioManager.PlayButtonPress);

        MenuButtonPropagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        
        JingJieAnnotationProvider.SetAddress(new("Run.Environment.JingJieDescription"));
        DifficultyAnnotationProvider.SetAddress(new("Run.Environment.DifficultyDescription"));
    }

    private void OnEnable()
    {
        MingYuan.Configure(1, RunManager.Instance.Environment.GetMingYuan, "Run.Environment.MingYuanDescription");
        Gold.Configure(1, RunManager.Instance.Environment.GetGold, "Run.Environment.GoldDescription");
        Health.Configure(1, RunManager.Instance.Environment.Home.GetHealthBounded, "Run.Environment.HealthDescription");
        
        bool allowRotate = RunManager.Instance.Environment.GetRunConfig().DifficultyProfile.GetEntry().AllowRotate;
        CycleIcon.gameObject.SetActive(allowRotate);
        
        RunManager.Instance.Environment.GainMingYuanNeuron.Add(GainMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(LoseMingYuan);
        RunManager.Instance.Environment.GainGoldNeuron.Add(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(LoseGold);
        RunManager.Instance.Environment.GainHealthNeuron.Add(GainHealth);
        RunManager.Instance.Environment.LoseHealthNeuron.Add(LoseHealth);
        
        RunManager.Instance.Environment.JingJieChangedNeuron.Add(RefreshJingJieText);
        
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
        JingJieText.text = $"{d.ToJingJie.GetName()}期";
    }

    public void Refresh()
    {
        MingYuan.Refresh();
        Gold.Refresh();
        Health.Refresh();
        
        DifficultyText.text = $"难度{RunManager.Instance.Environment.GetRunConfig().GetDifficulty()}";
        JingJieText.text = $"{RunManager.Instance.Environment.JingJie.GetName()}期";
    }

    public void OpenMenu()
    {
        AppManager.Instance.Push(AppStateMachine.MENU);
    }
}
