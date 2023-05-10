
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    public EntityView EnemyView;
    public EntityView HeroView;
    public SkillInventoryView SkillInventoryView;

    public TMP_Text SimulatedHP;
    public Image Light;
    public Button ReportButton;
    public Button StreamButton;
    public TMP_Text ReportText;

    public override void Configure()
    {
        base.Configure();
        HeroView.Configure(new IndexPath("Battle.Hero"));
        EnemyView.Configure(new IndexPath("Battle.Enemy"));
        SkillInventoryView.Configure(new IndexPath("Battle.SkillInventory"));

        ReportButton.onClick.AddListener(Report);
        StreamButton.onClick.AddListener(Stream);
    }

    public override void Refresh()
    {
        HeroView.Refresh();
        EnemyView.Refresh();
        SkillInventoryView.Refresh();

        if (RunManager.Instance.Battle.Report is { } report)
        {
            SimulatedHP.text = $"玩家 : 怪物\n{report.HomeLeftHp} : {report.AwayLeftHp}";
            Light.color = report.HomeVictory ? Color.green : Color.red;
            ReportText.text = report.ToString();
        }
        else
        {
            SimulatedHP.text = $"玩家 : 怪物\n无结果";
            Light.color = Color.gray;
            ReportText.text = "";
        }
    }

    private void Report()
    {
        RunManager.Instance.Combat(false, RunManager.Instance.Battle);
        RunCanvas.Instance.Refresh();
    }

    private void Stream()
    {
        RunManager.Instance.Combat(true, RunManager.Instance.Battle);
        RunCanvas.Instance.Refresh();
    }
}
