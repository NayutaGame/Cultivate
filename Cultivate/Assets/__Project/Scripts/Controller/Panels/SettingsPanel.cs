
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private ListView Widgets;

    [SerializeField] private Button CurrButton;
    [SerializeField] private Button[] OtherButtons;

    [SerializeField] private TMP_Text CurrLabel;
    [SerializeField] private TMP_Text[] OtherLabels;

    [SerializeField] private Button ToTitleButton;
    [SerializeField] private Button ToDesktopButton;
    [SerializeField] private Button ResumeButton;

    private Address _address;
    public void Configure()
    {
        _address = new Address("App.Settings");
        Settings settings = _address.Get<Settings>();

        ResumeButton.onClick.RemoveAllListeners();
        ResumeButton.onClick.AddListener(Resume);

        for (int i = 0; i < OtherButtons.Length; i++)
        {
            int index = i;
            OtherButtons[i].onClick.RemoveAllListeners();
            OtherButtons[i].onClick.AddListener(() =>
            {
                settings.ChangeIndex(index);
                Refresh();
            });
        }

        Widgets.SetPrefabProvider(model =>
        {
            WidgetModel widgetModel = (WidgetModel)model;
            if (widgetModel is SliderModel)
                return 0;
            if (widgetModel is SwitchModel)
                return 1;
            if (widgetModel is CheckboxModel)
                return 2;
            if (widgetModel is ButtonModel)
                return 3;
            return -1;
        });
        Widgets.SetAddress(_address.Append(".CurrentWidgets"));
        Widgets.Refresh();
    }

    public void Refresh()
    {
        Settings settings = _address.Get<Settings>();
        CurrLabel.text = $"<rotate=90>{settings.GetCurrentContentModel().Name}";

        for (int i = 0; i < settings.GetOtherContentCount(); i++)
        {
            SettingsContentModel content = settings.GetOtherContent(i);
            OtherLabels[i].text = content.Name;
        }

        Widgets.Refresh();
    }

    private void Resume()
    {
        AppManager.Pop();
    }
}
