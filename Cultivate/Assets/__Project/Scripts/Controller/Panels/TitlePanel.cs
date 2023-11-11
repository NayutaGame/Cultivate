
using UnityEngine.UI;

public class TitlePanel : CurtainPanel
{
    public Button StartRunButton;
    public Button SettingsButton;
    public Button ExitButton;

    public override void Configure()
    {
        base.Configure();

        StartRunButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        StartRunButton.onClick.AddListener(StartRun);
        SettingsButton.onClick.AddListener(OpenMenu);
    }

    private void StartRun()
    {
        AppManager.Push(new RunAppS());
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }
}
