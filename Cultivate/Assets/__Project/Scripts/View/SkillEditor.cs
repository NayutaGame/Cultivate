using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class SkillEditor : MonoBehaviour
{
    public Transform HeroNeiGongTransform;
    public Transform HeroWaiGongTransform;

    public EnemyView EnemyView;

    public TMP_InputField HeroHPInputField;

    public TMP_Text SimulatedPlayerHP;
    public TMP_Text SimulatedEnemyHP;

    private RunChipView[] _heroNeiGongViews;
    private RunChipView[] _heroWaiGongViews;

    public void Configure()
    {
        ConfigureList(ref _heroWaiGongViews, RunManager.WaiGongLimit, HeroWaiGongTransform, "Hero.HeroSlotInventory.Slots");
        EnemyView.Configure(new IndexPath("Enemy"));

        HeroHPInputField.text = RunManager.Instance.Hero.Health.ToString();
        HeroHPInputField.onEndEdit.AddListener(SetHeroHP);
    }

    public void Refresh()
    {
        foreach(var view in _heroWaiGongViews) view.Refresh();
        EnemyView.Refresh();

        HeroHPInputField.text = RunManager.Instance.Hero.Health.ToString();

        SimulatedPlayerHP.text = $"玩家剩余生命: {RunManager.Instance.Report.HomeLeftHp}";
        SimulatedEnemyHP.text = $"怪物剩余生命: {RunManager.Instance.Report.AwayLeftHp}";
    }

    private void ConfigureList(ref RunChipView[] views, int limit, Transform parentTransform, string indexPathString)
    {
        views = new RunChipView[limit];
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            RunChipView view = parentTransform.GetChild(i).GetComponent<RunChipView>();
            views[i] = view;
            view.Configure(new IndexPath($"{indexPathString}#{i}"));
        }
    }

    private void SetHeroHP(string hpString)
    {
        int hp = int.Parse(hpString);
        hp = Mathf.Clamp(hp, 1, 9999);
        RunManager.Instance.Hero.Health = hp;
    }
}
