
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text TurnCountText;
    
    [SerializeField] private StagePanelSpeedButton SpeedButton;
    [SerializeField] private Button4State SkipButton;

    [SerializeField] private StageEntityView HomeStageEntityView;
    [SerializeField] private StageEntityView AwayStageEntityView;

    // [SerializeField] private XView SkipButtonInactiveHint;

    public TimelineView TimelineView;

    private Address _address;

    private int _speedIndex;
    private int SpeedIndex
    {
        get => _speedIndex;
        set
        {
            _speedIndex = (value + SPEED_CALI.Length) % SPEED_CALI.Length;

            float speed = SPEED_CALI[_speedIndex];
            SpeedButton.Text.text = $"x{speed}";
            StageManager.Instance.SetSpeed(speed);
        }
    }

    private void IncreaseSpeed(InteractBehaviour ib, PointerEventData d) => SpeedIndex++;
    private void DecreaseSpeed(InteractBehaviour ib, PointerEventData d) => SpeedIndex--;
    private void ResetSpeed() => SpeedIndex = 1;
    
    private readonly float[] SPEED_CALI = new float[] { 0.5f, 1f, 2f, 4f, 8f };

    public void Configure()
    {
        _address = new Address("Stage");
        // _animationQueue = new();

        SpeedButton.CheckAwake();
        SpeedButton.LeftClickNeuron.Join(IncreaseSpeed);
        SpeedButton.RightClickNeuron.Join(DecreaseSpeed);
        ResetSpeed();
        
        SkipButton.CheckAwake();
        SkipButton.LeftClickNeuron.Join(Skip);

        SkipButton.SetStateToInactiveFrom(!AppManager.Instance.ProfileManager.GetCurrProfile().DifficultyIsUnlocked("1"));

        HomeStageEntityView.SetAddress(_address.Append(".Environment.Home"));
        AwayStageEntityView.SetAddress(_address.Append(".Environment.Away"));
        
        // SkipButtonInactiveHint.SetAddress(_address.Append(".SkipButtonInactiveHint"));

        TimelineView.Configure();
    }

    public void Refresh()
    {
        TurnCountText.text = "0";
        HomeStageEntityView.Refresh();
        AwayStageEntityView.Refresh();
    }

    private void OnEnable()
    {
        bool hasClearDifficulty0 = AppManager.Instance.ProfileManager.GetCurrProfile().DifficultyIsUnlocked("1");
        if (hasClearDifficulty0)
            AppManager.Instance.PushEscFunc(Skip);
        else
            AppManager.Instance.PushEscFunc(Empty);
    }

    private void OnDisable()
    {
        AppManager.Instance.PopEscFunc();
    }
    
    private void Empty() { }

    private void Skip()
        => StageManager.Instance.Skip();

    private void Skip(InteractBehaviour ib, PointerEventData d)
        => StageManager.Instance.Skip();

    public void InitialSetup()
    {
        TimelineView.InitialSetup();

        ResetSpeed();
        Refresh();
    }

    public void TurnCountChangedStaging(int turnCount)
    {
        TurnCountText.text = turnCount.ToString();
    }

    public void GainBuffStaging(bool tgtIsHome)
    {
        StageEntityView entityView = tgtIsHome ? HomeStageEntityView : AwayStageEntityView;
        entityView.Buffs.AddItem();
        entityView.Buffs.ForceLayoutRebuild();
        
        entityView.Buffs.LastView().Align();
    }
    
    public void LoseBuffStaging(bool tgtIsHome, int buffIndex)
    {
        StageEntityView entityView = tgtIsHome ? HomeStageEntityView : AwayStageEntityView;
        entityView.Buffs.RemoveItemAt(buffIndex);
        entityView.Buffs.ForceLayoutRebuild();

        int i = 0;
        entityView.Buffs.Traversal().Do(slotView =>
        {
            if (i >= buffIndex)
                slotView.Align();
            i++;
        });
        
        // 应急处理，因为动画的延迟，emphasize会导致desync，emphasize做成过程，将当时的状态发给view层才是正确做法
        entityView.Buffs.Refresh();
    }

    public void GainFormationStaging(bool ownerIsHome)
    {
        StageEntityView entityView = ownerIsHome ? HomeStageEntityView : AwayStageEntityView;
        entityView.Formations.AddItem();
        entityView.Formations.ForceLayoutRebuild();
        
        entityView.Formations.LastView().Align();
    }
}
