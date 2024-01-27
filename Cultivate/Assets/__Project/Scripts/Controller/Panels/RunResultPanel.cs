
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunResultPanel : Panel
{
    [SerializeField] private TMP_Text ResultText;
    [SerializeField] private Button ReturnButton;

    public override void Configure()
    {
        base.Configure();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);
    }

    public override void Refresh()
    {
        base.Refresh();

        RunResultPanelDescriptor panelDescriptor = RunManager.Instance.Environment.GetActivePanel() as RunResultPanelDescriptor;
        if (panelDescriptor.RunResult.State == RunResult.RunResultState.Victory)
        {
            ResultText.text = "胜利";
        }
        else
        {
            ResultText.text = "失败";
        }
    }

    private void Return()
    {
        RunManager.Instance.ReturnToTitle();
    }
}
