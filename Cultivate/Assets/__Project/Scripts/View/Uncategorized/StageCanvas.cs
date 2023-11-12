
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageCanvas : MonoBehaviour
{
    public TMP_Text SpeedText;
    public Slider SpeedSlider;
    public Button SkipButton;

    [SerializeField] private StageEntityView HomeStageEntityView;
    [SerializeField] private StageEntityView AwayStageEntityView;

    public TimelineView TimelineView;

    private Address _address;

    public void Configure()
    {
        _address = new Address("Stage");

        SpeedSlider.onValueChanged.RemoveAllListeners();
        SpeedSlider.onValueChanged.AddListener(SpeedChanged);

        SkipButton.onClick.RemoveAllListeners();
        SkipButton.onClick.AddListener(Skip);

        HomeStageEntityView.SetAddress(_address.Append(".Environment.Home"));
        AwayStageEntityView.SetAddress(_address.Append(".Environment.Away"));

        TimelineView.Configure();
        // _address.Append(".Timeline");

        ConfigureInteractDelegate();
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(2,
            getId: view =>
            {
                if (view is BuffView)
                    return 0;
                if (view is StageFormationIconView)
                    return 1;
                return null;
            });

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((BuffView)v).PointerEnter(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((BuffView)v).PointerExit(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((BuffView)v).PointerMove(v, d));

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 1, (v, d) => ((StageFormationIconView)v).PointerEnter(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 1, (v, d) => ((StageFormationIconView)v).PointerExit(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 1, (v, d) => ((StageFormationIconView)v).PointerMove(v, d));

        HomeStageEntityView.SetDelegate(InteractDelegate);
        AwayStageEntityView.SetDelegate(InteractDelegate);
    }

    #endregion

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
}
