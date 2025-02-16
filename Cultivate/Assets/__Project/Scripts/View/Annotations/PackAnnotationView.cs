
using TMPro;
using UnityEngine;

public class PackAnnotationView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text WuXingText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        IPack pack = Get<IPack>();

        if (pack == null)
        {
            gameObject.SetActive(false);
            return;
        }

        NameText.text = pack.GetName();
        SetWuXing(pack.GetWuXing());
        DescriptionText.text = pack.GetDescription();
        SetTrivia(pack.GetTrivia());
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }

    private void SetWuXing(WuXing? wuXing)
    {
        WuXingText.text = wuXing?.ToString() ?? "无";
    }
}
