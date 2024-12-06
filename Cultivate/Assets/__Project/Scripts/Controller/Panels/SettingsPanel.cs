
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanel : Panel
{
    [SerializeField] private CanvasGroup CoreCanvasGroup;
    [SerializeField] private Image DarkCurtainImage;
    [SerializeField] private Button DarkCurtainButton;

    [SerializeField] private LegacyListView WidgetListView;
    [SerializeField] public Transform WidgetsTransform;
    [SerializeField] public CanvasGroup WidgetsCanvasGroup;

    [SerializeField] private LegacyListView TabListView;

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
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Settings");
        Settings settings = _address.Get<Settings>();
        settings.ResetSelectedTab();

        ResumeButton.onClick.RemoveAllListeners();
        ResumeButton.onClick.AddListener(Resume);

        DarkCurtainButton.onClick.RemoveAllListeners();
        DarkCurtainButton.onClick.AddListener(Resume);

        ToTitleButton.onClick.RemoveAllListeners();
        ToTitleButton.onClick.AddListener(ToTitle);

        ToDesktopButton.onClick.RemoveAllListeners();
        ToDesktopButton.onClick.AddListener(ToDesktop);
        
        TabListView.SetAddress(_address.Append(".Tabs"));
        TabListView.LeftClickNeuron.Join(ClickedTab);

        WidgetListView.SetPrefabProvider(model =>
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
        WidgetListView.SetAddress(_address.Append(".CurrentWidgets"));
        WidgetListView.Refresh();
    }

    public override void Refresh()
    {
        base.Refresh();
        TabListView.Refresh();
        WidgetListView.Refresh();
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

    public override Tween EnterIdle()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Join(GetRect().DOScale(1f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CoreCanvasGroup.DOFade(1f, 0.15f))
            .Join(DarkCurtainImage.DOFade(0.7f, 0.15f));
    }

    public override Tween EnterHide()
    {
        return DOTween.Sequence()
            .Join(GetRect().DOScale(1.4f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CoreCanvasGroup.DOFade(0f, 0.15f))
            .Join(DarkCurtainImage.DOFade(0f, 0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }

    private Tween _handle;

    private void ClickedTab(LegacyInteractBehaviour toIb, PointerEventData d)
    {
        SettingsTab fromTab = AppManager.Instance.Settings.GetSelectedTab();
        SettingsTab toTab = toIb.GetSimpleView().Get<SettingsTab>();

        if (fromTab == toTab)
            return;
        
        AppManager.Instance.Settings.SetSelectedTab(toTab);
        
        // Staging
        SettingsTabView fromTabView = TabListView.ActivePool[AppManager.Instance.Settings.FindIndexOfTab(fromTab)]
            .GetInteractBehaviour().GetCLView() as SettingsTabView;
        SettingsTabView toTabView = toIb.GetCLView() as SettingsTabView;
        
        _handle?.Kill();
        _handle = TabChangedAnimation(fromTabView, toTabView);
        _handle.SetAutoKill().Restart();
        
        fromTabView.Unselect();
        toTabView.Select();
    }

    public Tween TabChangedAnimation(SettingsTabView fromTabView, SettingsTabView toTabView)
    {
        return DOTween.Sequence()
            .Join(DOTween.Sequence()
                .Append(WidgetsCanvasGroup.DOFade(0.4f, 0.075f).SetEase(Ease.OutQuad))
                .Join(WidgetsTransform.DOScale(0.9f, 0.075f))
                .AppendCallback(() => WidgetListView.Sync())
                .Append(WidgetsTransform.DOScale(1f, 0.075f))
                .Join(WidgetsCanvasGroup.DOFade(1, 0.075f)).SetEase(Ease.InQuad));
    }
}
