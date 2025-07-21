
using TMPro;
using UnityEngine;

public class LegacyPackAnnotationView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text WuXingText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        IPack pack = Get<IPack>();

        if (pack != null)
        {
            ConfigAsPack(pack);
            gameObject.SetActive(true);
            return;
        }

        PackConstraint packConstraint = Get<PackConstraint>();

        if (packConstraint != null)
        {
            pack = packConstraint.Pack;
            if (pack != null)
            {
                ConfigAsPack(pack);
                gameObject.SetActive(true);
                return;
            }
        }
        
        gameObject.SetActive(false);
    }

    private void ConfigAsPack(IPack pack)
    {
        NameText.text = pack.GetName();
        SetWuXing(pack.GetWuXing());
        DescriptionText.text = pack.GetDescription();
        SetTrivia(pack.GetTrivia());
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }

    private void SetWuXing(WuXing? wuXing)
    {
        WuXingText.text = wuXing?.ToString() ?? "无";
    }
}
