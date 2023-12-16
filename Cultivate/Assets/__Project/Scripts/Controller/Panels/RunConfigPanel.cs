
using UnityEngine;
using UnityEngine.UI;

public class RunConfigPanel : CurtainPanel
{
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button StartRunButton;

    public override void Configure()
    {
        base.Configure();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);

        StartRunButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.AddListener(StartRun);
    }

    public override void Refresh()
    {
        base.Refresh();
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
