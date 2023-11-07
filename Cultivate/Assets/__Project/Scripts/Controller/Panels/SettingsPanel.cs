
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Panel
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private ListView Widgets;

    [SerializeField] private RectTransform[] TabRectTransforms;
    [SerializeField] private Button[] TabButtons;
    [SerializeField] private TMP_Text[] TabLabels;

    [SerializeField] private RectTransform CurrentTabTransform;

    [SerializeField] private Button ToTitleButton;
    [SerializeField] private Button ToDesktopButton;
    [SerializeField] private Button ResumeButton;

    private void Awake()
    {
        // Tween t = HideAnimation().SetAutoKill();
        // t.Restart();
        // t.Complete();
    }

    private Address _address;
    public override void Configure()
    {
        base.Configure();
        _address = new Address("App.Settings");

        ResumeButton.onClick.RemoveAllListeners();
        ResumeButton.onClick.AddListener(Resume);

        for (int i = 0; i < TabButtons.Length; i++)
        {
            int index = i;
            TabButtons[i].onClick.RemoveAllListeners();
            TabButtons[i].onClick.AddListener(() => ClickedTab(index));
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

    private void ClickedTab(int index)
    {
        Settings settings = _address.Get<Settings>();
        settings.ChangeIndex(index);

        if (_handle != null)
            _handle.Kill();
        _handle = TabChangedAnimation(index);
        _handle.Restart();

        Refresh();
    }

    private Tween _handle;

    public Tween TabChangedAnimation(int index)
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(CurrentTabTransform.DOAnchorPos(TabRectTransforms[index].anchoredPosition, 0.15f));
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
