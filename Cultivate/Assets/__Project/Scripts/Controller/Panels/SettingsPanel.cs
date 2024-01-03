
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Panel
{
    [SerializeField] private CanvasGroup CoreCanvasGroup;
    [SerializeField] private Image DarkCurtainImage;
    [SerializeField] private Button DarkCurtainButton;

    [SerializeField] private ListView Widgets;
    [SerializeField] private Transform WidgetsTransform;
    [SerializeField] private CanvasGroup WidgetsCanvasGroup;

    [SerializeField] private Transform[] TabRectTransforms;
    [SerializeField] private Button[] TabButtons;
    [SerializeField] private TMP_Text[] TabLabels;

    [SerializeField] private Transform CurrentTabTransform;
    [SerializeField] private TMP_Text CurrentTabLabel;

    [SerializeField] private Button ToTitleButton;
    [SerializeField] private Button ToDesktopButton;
    [SerializeField] private Button ResumeButton;

    public void ShowExitButtons()
    {
        ToTitleButton.gameObject.SetActive(true);
        ToDesktopButton.gameObject.SetActive(true);
    }

    public void HideExitButtons()
    {
        ToTitleButton.gameObject.SetActive(false);
        ToDesktopButton.gameObject.SetActive(false);
    }

    private Address _address;
    public override void Configure()
    {
        base.Configure();
        _address = new Address("Settings");

        ResumeButton.onClick.RemoveAllListeners();
        ResumeButton.onClick.AddListener(Resume);

        DarkCurtainButton.onClick.RemoveAllListeners();
        DarkCurtainButton.onClick.AddListener(Resume);

        ToTitleButton.onClick.RemoveAllListeners();
        ToTitleButton.onClick.AddListener(ToTitle);

        ToDesktopButton.onClick.RemoveAllListeners();
        ToDesktopButton.onClick.AddListener(ToDesktop);

        Settings settings = _address.Get<Settings>();
        settings.ChangeIndex(0);

        for (int i = 0; i < TabButtons.Length; i++)
        {
            int index = i;
            TabButtons[i].onClick.RemoveAllListeners();
            TabButtons[i].onClick.AddListener(() => ClickedTab(index));
            TabLabels[i].text = settings.GetLabelForIndex(i);
        }

        Widgets.SetPrefabProvider(model =>
        {
            WidgetModel widgetModel = (WidgetModel)model;
            if (widgetModel is SliderModel)
                return 0;
            if (widgetModel is SwitchModel)
                return 1;
            if (widgetModel is ToggleModel)
                return 2;
            if (widgetModel is ButtonModel)
                return 3;
            return -1;
        });
        Widgets.SetAddress(_address.Append(".CurrentWidgets"));
        Widgets.Refresh();
    }

    public override void Refresh()
    {
        base.Refresh();
        Widgets.Refresh();
    }

    private void Resume()
    {
        AppManager.Pop();
    }

    private void ToTitle()
    {
        AppManager.Pop(2);
    }

    private void ToDesktop()
    {
        AppManager.ExitGame();
    }

    private Tween _handle;

    private void ClickedTab(int index)
    {
        Settings settings = _address.Get<Settings>();
        settings.ChangeIndex(index);

        Widgets.SetAddress(_address.Append(".CurrentWidgets"));

        _handle?.Kill();
        _handle = TabChangedAnimation(index, settings.GetCurrentContentModel().Name);
        _handle.Restart();
    }

    public Tween TabChangedAnimation(int index, string newLabel)
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(CurrentTabTransform.DOMove(TabRectTransforms[index].position, 0.15f))
            .Join(DOTween.Sequence()
                .Append(CurrentTabLabel.DOFade(0, 0.075f))
                .AppendCallback(() => CurrentTabLabel.text = newLabel)
                .Append(CurrentTabLabel.DOFade(1, 0.075f)))
            .Join(DOTween.Sequence()
                .Append(WidgetsCanvasGroup.DOFade(0.4f, 0.075f).SetEase(Ease.OutQuad))
                .Join(WidgetsTransform.DOScale(0.9f, 0.075f))
                .AppendCallback(Refresh)
                .Append(WidgetsTransform.DOScale(1f, 0.075f))
                .Join(WidgetsCanvasGroup.DOFade(1, 0.075f)).SetEase(Ease.InQuad));
    }

    public override Tween ShowAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(RectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CoreCanvasGroup.DOFade(1f, 0.15f))
            .Join(DarkCurtainImage.DOFade(0.2f, 0.15f));
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(RectTransform.DOScale(1.2f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CoreCanvasGroup.DOFade(0f, 0.15f))
            .Join(DarkCurtainImage.DOFade(0f, 0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
