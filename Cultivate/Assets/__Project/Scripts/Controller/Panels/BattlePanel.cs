
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    [SerializeField] private BattleEntityView EnemyView;

    public TMP_Text HomeHP;
    public GameObject HomeHPSlash;
    public TMP_Text AwayHP;
    public GameObject AwayHPSlash;

    public Button CombatButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        EnemyView.SetAddress(_address.Append(".Enemy"));

        CombatButton.onClick.RemoveAllListeners();
        CombatButton.onClick.AddListener(Combat);
    }

    public override void Refresh()
    {
        EnemyView.Refresh();

        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();

        if (d.GetResult() is { } result)
        {
            HomeHP.text = result.HomeLeftHp.ToString();
            AwayHP.text = result.AwayLeftHp.ToString();
            if (result.HomeVictory)
            {
                HomeHP.color = Color.white;
                HomeHPSlash.SetActive(false);
                AwayHP.color = Color.black;
                AwayHPSlash.SetActive(true);
            }
            else
            {
                HomeHP.color = Color.black;
                HomeHPSlash.SetActive(true);
                AwayHP.color = Color.white;
                AwayHPSlash.SetActive(false);
            }
        }
        else
        {
            HomeHP.text = "玩家";
            AwayHP.text = "怪物";
            HomeHP.color = Color.white;
            HomeHPSlash.SetActive(false);
            AwayHP.color = Color.white;
            AwayHPSlash.SetActive(false);
        }
    }

    private void Combat()
    {
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat();
        RunCanvas.Instance.Refresh();
    }
}
