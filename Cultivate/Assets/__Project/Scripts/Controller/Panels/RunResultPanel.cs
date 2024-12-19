
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RunResultPanel : Panel
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private Button ReturnButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);
    }

    public override void Refresh()
    {
        base.Refresh();

        RunResultPanelDescriptor panelDescriptor = RunManager.Instance.Environment.GetActivePanel() as RunResultPanelDescriptor;
        if (panelDescriptor.RunResult.GetState() == RunResult.RunResultState.Victory)
        {
            TitleText.text = "胜利";
        }
        else
        {
            TitleText.text = "失败";
        }
    }

    private void Return()
    {
        RunManager.Instance.ReturnToTitle();
    }
}
