
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    public Button StartRunButton;
    public Button SettingsButton;
    public Button ExitButton;

    public void Configure()
    {
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

    public void Refresh()
    {

    }
}
