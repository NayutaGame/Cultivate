
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    public Button StartRunButton;
    public Button TutorialButton;
    public Button SettingsButton;
    public Button ExitButton;

    public void Configure()
    {
        StartRunButton.onClick.RemoveAllListeners();
        TutorialButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        StartRunButton.onClick.AddListener(StartRun);
        TutorialButton.onClick.AddListener(OpenTutorial);
        SettingsButton.onClick.AddListener(OpenMenu);
    }

    private void StartRun()
    {
        AppManager.Push(new RunAppS());
    }

    private void OpenTutorial()
    {
        AppCanvas.Instance.TutorialPanel.gameObject.SetActive(true);
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }

    public void Refresh()
    {

    }
}
