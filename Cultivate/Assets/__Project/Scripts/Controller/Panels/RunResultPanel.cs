
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

        // get run result and bind
    }

    private void Return()
    {

    }
}
