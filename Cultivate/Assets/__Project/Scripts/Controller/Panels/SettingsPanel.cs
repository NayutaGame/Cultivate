
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanel : PopupPanel
{
    [SerializeField] private LegacyListView WidgetListView;
    [SerializeField] public Transform WidgetsTransform;
    [SerializeField] public CanvasGroup WidgetsCanvasGroup;

    [SerializeField] private LegacyListView TabListView;

    [SerializeField] private Button4State ToTitleButton;
    [SerializeField] private Button4State ToDesktopButton;
    [SerializeField] private Button4State ResumeButton;

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

    private void OnEnable()
    {
        ToTitleButton.LeftClickNeuron.Add(ToTitle);
        ToDesktopButton.LeftClickNeuron.Add(ToDesktop);
        ResumeButton.LeftClickNeuron.Add(Return);
        
        AudioManager.PlayEnterSettings();
        
        AppManager.Instance.PushEscFunc(Return);
    }

    private void OnDisable()
    {
        ToTitleButton.LeftClickNeuron.Remove(ToTitle);
        ToDesktopButton.LeftClickNeuron.Remove(ToDesktop);
        ResumeButton.LeftClickNeuron.Remove(Return);
        
        AudioManager.PlayExitSettings();
        
        AppManager.Instance.PopEscFunc();
    }

    private Address _address;
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Settings");
        Settings settings = _address.Get<Settings>();
        settings.ResetSelectedTab();
        
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
        TabListView.Refresh();
        WidgetListView.Refresh();
    }

    private void Return(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.Settings.SaveProcedure();
        AppManager.Instance.Pop();
    }

    private void ToTitle(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.Settings.SaveProcedure();
        AppManager.Instance.Pop(2);
    }

    private void ToDesktop(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.Settings.SaveProcedure();
        AppManager.ExitGame();
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
