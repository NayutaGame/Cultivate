
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Panel
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private ListView Widgets;

    [SerializeField] private Button CurrButton;
    [SerializeField] private Button[] OtherButtons;

    [SerializeField] private TMP_Text CurrLabel;
    [SerializeField] private TMP_Text[] OtherLabels;

    [SerializeField] private Button ToTitleButton;
    [SerializeField] private Button ToDesktopButton;
    [SerializeField] private Button ResumeButton;

    private void Awake()
    {
        Tween t = HideAnimation().SetAutoKill();
        t.Restart();
        t.Complete();
    }

    private Address _address;
    public override void Configure()
    {
        // _address = new Address("App.Settings");
        // Settings settings = _address.Get<Settings>();
        //
        // ResumeButton.onClick.RemoveAllListeners();
        // ResumeButton.onClick.AddListener(Resume);
        //
        // for (int i = 0; i < OtherButtons.Length; i++)
        // {
        //     int index = i;
        //     OtherButtons[i].onClick.RemoveAllListeners();
        //     OtherButtons[i].onClick.AddListener(() =>
        //     {
        //         settings.ChangeIndex(index);
        //         Refresh();
        //     });
        // }
        //
        // Widgets.SetPrefabProvider(model =>
        // {
        //     WidgetModel widgetModel = (WidgetModel)model;
        //     if (widgetModel is SliderModel)
        //         return 0;
        //     if (widgetModel is SwitchModel)
        //         return 1;
        //     if (widgetModel is CheckboxModel)
        //         return 2;
        //     if (widgetModel is ButtonModel)
        //         return 3;
        //     return -1;
        // });
        // Widgets.SetAddress(_address.Append(".CurrentWidgets"));
        // Widgets.Refresh();
    }

    public override void Refresh()
    {
        // Settings settings = _address.Get<Settings>();
        // CurrLabel.text = $"<rotate=90>{settings.GetCurrentContentModel().Name}";
        //
        // for (int i = 0; i < settings.GetOtherContentCount(); i++)
        // {
        //     SettingsContentModel content = settings.GetOtherContent(i);
        //     OtherLabels[i].text = content.Name;
        // }
        //
        // Widgets.Refresh();
    }

    private void Resume()
    {
        AppManager.Pop();
    }

    public override Tween ShowAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(_rectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad))
            .Join(_canvasGroup.DOFade(1f, 0.15f));
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(_rectTransform.DOScale(1.2f, 0.15f).SetEase(Ease.OutQuad))
            .Join(_canvasGroup.DOFade(0f, 0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
