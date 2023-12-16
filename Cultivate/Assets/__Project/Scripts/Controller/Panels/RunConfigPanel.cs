
using UnityEngine;
using UnityEngine.UI;

public class RunConfigPanel : CurtainPanel
{
    [SerializeField] private ListView CharacterListView;
    // Random Character Button

    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button StartRunButton;

    public override void Configure()
    {
        base.Configure();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);

        StartRunButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.AddListener(StartRun);

        CharacterListView.SetAddress(new Address("Profile.ProfileList.Current.CharacterProfileList"));
    }

    public override void Refresh()
    {
        base.Refresh();
        CharacterListView.Refresh();
    }

    private void Return()
    {
        CanvasManager.Instance.AppCanvas.CloseRunConfigPanel();
    }

    private void StartRun()
    {
        AppManager.Push(new RunAppS());
    }
}
