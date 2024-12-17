
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageCanvas : MonoBehaviour
{
    public TMP_Text SpeedText;
    public Slider SpeedSlider;
    // public Button SkipButton;

    [SerializeField] private StageEntityView HomeStageEntityView;
    [SerializeField] private StageEntityView AwayStageEntityView;

    public TimelineView TimelineView;

    private Address _address;

    public void Configure()
    {
        _address = new Address("Stage");
        // _animationQueue = new();

        SpeedSlider.onValueChanged.RemoveAllListeners();
        SpeedSlider.onValueChanged.AddListener(SpeedChanged);

        // SkipButton.onClick.RemoveAllListeners();
        // SkipButton.onClick.AddListener(Skip);

        HomeStageEntityView.SetAddress(_address.Append(".Environment.Home"));
        AwayStageEntityView.SetAddress(_address.Append(".Environment.Away"));

        TimelineView.Configure();
    }

    public void Refresh()
    {
        HomeStageEntityView.Refresh();
        AwayStageEntityView.Refresh();
    }

    private void SpeedChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        float speed;
        if (intValue == -1)
        {
            speed = 0.5f;
            SpeedText.text = "0.5倍速";
        }
        else
        {
            speed = Mathf.Pow(2, intValue);
            SpeedText.text = $"{Mathf.RoundToInt(speed)}倍速";
        }

        StageManager.Instance.SetSpeed(speed);
    }

    private void Skip()
    {
        StageManager.Instance.Skip();
    }

    public void InitialSetup()
    {
        TimelineView.InitialSetup();

        SpeedSlider.value = 0;
        Refresh();
    }

    public void GainBuffStaging(bool tgtIsHome)
    {
        StageEntityView entityView = tgtIsHome ? HomeStageEntityView : AwayStageEntityView;
        entityView.Buffs.AddItem();
        entityView.Buffs.ForceLayoutRebuild();
        
        (entityView.Buffs.LastView() as DelegatingView).Align();
    }
    
    public void LoseBuffStaging(bool tgtIsHome, int buffIndex)
    {
        StageEntityView entityView = tgtIsHome ? HomeStageEntityView : AwayStageEntityView;
        entityView.Buffs.RemoveItemAt(buffIndex);
        entityView.Buffs.ForceLayoutRebuild();

        int i = 0;
        entityView.Buffs.Traversal().Do(v =>
        {
            if (i >= buffIndex)
                (v as DelegatingView).Align();
            i++;
        });
    }

    public void GainFormationStaging(bool ownerIsHome)
    {
        StageEntityView entityView = ownerIsHome ? HomeStageEntityView : AwayStageEntityView;
        entityView.Formations.AddItem();
        entityView.Formations.ForceLayoutRebuild();
        
        (entityView.Formations.LastView() as DelegatingView).Align();
    }
}
