using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEditor : MonoBehaviour
{
    public Transform HeroNeiGongTransform;
    public Transform HeroWaiGongTransform;
    public Transform EnemyNeiGongTransform;
    public Transform EnemyWaiGongTransform;

    private RunChipView[] _heroNeiGongViews;
    private RunChipView[] _heroWaiGongViews;
    private RunChipView[] _enemyNeiGongViews;
    private RunChipView[] _enemyWaiGongViews;

    public void Configure()
    {
        ConfigureList(ref _heroNeiGongViews, RunManager.NeiGongLimit, HeroNeiGongTransform, "GetHeroNeiGong");
        ConfigureList(ref _heroWaiGongViews, RunManager.WaiGongLimit, HeroWaiGongTransform, "GetHeroWaiGong");
        ConfigureList(ref _enemyNeiGongViews, RunManager.NeiGongLimit, EnemyNeiGongTransform, "GetEnemyNeiGong");
        ConfigureList(ref _enemyWaiGongViews, RunManager.WaiGongLimit, EnemyWaiGongTransform, "GetEnemyWaiGong");
    }

    public void Refresh()
    {
        foreach(var view in _heroNeiGongViews) view.Refresh();
        foreach(var view in _heroWaiGongViews) view.Refresh();
        foreach(var view in _enemyNeiGongViews) view.Refresh();
        foreach(var view in _enemyWaiGongViews) view.Refresh();
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
}
