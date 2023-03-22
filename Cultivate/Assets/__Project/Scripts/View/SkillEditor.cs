using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class SkillEditor : MonoBehaviour
{
    public Transform HeroNeiGongTransform;
    public Transform HeroWaiGongTransform;
    public Transform EnemyNeiGongTransform;
    public Transform EnemyWaiGongTransform;

    public TMP_InputField HeroHPInputField;
    public TMP_InputField EnemyHPInputField;

    public TMP_Text SimulatedPlayerHP;
    public TMP_Text SimulatedEnemyHP;

    public TMP_Dropdown EnemyPicker;

    private RunChipView[] _heroNeiGongViews;
    private RunChipView[] _heroWaiGongViews;
    private RunChipView[] _enemyNeiGongViews;
    private RunChipView[] _enemyWaiGongViews;

    public void Configure()
    {
        ConfigureList(ref _heroWaiGongViews, RunManager.WaiGongLimit, HeroWaiGongTransform, "GetHeroSlot");
        ConfigureList(ref _enemyWaiGongViews, RunManager.WaiGongLimit, EnemyWaiGongTransform, "GetEnemySlot");

        HeroHPInputField.text = RunManager.Instance.Hero.Health.ToString();
        EnemyHPInputField.text = RunManager.Instance.Enemy.Health.ToString();
        HeroHPInputField.onEndEdit.AddListener(SetHeroHP);
        EnemyHPInputField.onEndEdit.AddListener(SetEnemyHP);

        if (EnemyPicker != null)
        {
            List<TMP_Dropdown.OptionData> options = new();
            Encyclopedia.EnemyCategory.Traversal.Do(enemy => options.Add(new TMP_Dropdown.OptionData(enemy.Name)));
            EnemyPicker.options = options;

            EnemyPicker.onValueChanged.AddListener(SetEnemy);
        }
    }

    public void Refresh()
    {
        foreach(var view in _heroWaiGongViews) view.Refresh();
        foreach(var view in _enemyWaiGongViews) view.Refresh();

        SimulatedPlayerHP.text = $"玩家剩余生命: {RunManager.Instance.Brief.Item1}";
        SimulatedEnemyHP.text = $"怪物剩余生命: {RunManager.Instance.Brief.Item2}";
    }

    private void ConfigureList(ref RunChipView[] views, int limit, Transform parentTransform, string indexPathString)
    {
        views = new RunChipView[limit];
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            RunChipView view = parentTransform.GetChild(i).GetComponent<RunChipView>();
            views[i] = view;
            view.Configure(new IndexPath(indexPathString, i));
        }
    }

    private void SetHeroHP(string hpString)
    {
        int hp = int.Parse(hpString);
        hp = Mathf.Clamp(hp, 1, 9999);
        RunManager.Instance.Hero.Health = hp;
    }

    private void SetEnemyHP(string hpString)
    {
        int hp = int.Parse(hpString);
        hp = Mathf.Clamp(hp, 1, 9999);
        RunManager.Instance.Enemy.Health = hp;
    }

    private void SetEnemy(int i)
    {
        CreateEnemyDetails d = new CreateEnemyDetails();
        RunEnemy e = Encyclopedia.EnemyCategory[i].Create(d);
        RunManager.Instance.SetEnemy(e);

        RunCanvas.Instance.Refresh();
    }
}
