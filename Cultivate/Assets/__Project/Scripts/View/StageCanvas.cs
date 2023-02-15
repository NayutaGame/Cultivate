using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class StageCanvas : Singleton<StageCanvas>
{
    public Transform HeroNeiGongContainerTransform;
    public Transform HeroWaiGongContainerTransform;
    public Transform EnemyNeiGongContainerTransform;
    public Transform EnemyWaiGongContainerTransform;

    public TMP_Text _heroHealthText;
    public TMP_Text _heroArmorText;
    public TMP_Text _enemyHealthText;
    public TMP_Text _enemyArmorText;

    private StageNeiGongView[] _heroNeiGongViews;
    private StageWaiGongView[] _heroWaiGongViews;
    private StageNeiGongView[] _enemyNeiGongViews;
    private StageWaiGongView[] _enemyWaiGongViews;

    public override void DidAwake()
    {
        base.DidAwake();
        Configure();
    }

    public void Configure()
    {
        _heroNeiGongViews = new StageNeiGongView[StageManager.Instance.GetHeroNeiGongCount()];
        for (int i = 0; i < HeroNeiGongContainerTransform.childCount; i++)
        {
            _heroNeiGongViews[i] = HeroNeiGongContainerTransform.GetChild(i).GetComponent<StageNeiGongView>();
            _heroNeiGongViews[i].Configure(new IndexPath("GetHeroStageNeiGong", i));
        }

        _heroWaiGongViews = new StageWaiGongView[StageManager.Instance.GetHeroWaiGongCount()];
        for (int i = 0; i < HeroWaiGongContainerTransform.childCount; i++)
        {
            _heroWaiGongViews[i] = HeroWaiGongContainerTransform.GetChild(i).GetComponent<StageWaiGongView>();
            _heroWaiGongViews[i].Configure(new IndexPath("GetHeroStageWaiGong", i));
        }

        _enemyNeiGongViews = new StageNeiGongView[StageManager.Instance.GetEnemyNeiGongCount()];
        for (int i = 0; i < EnemyNeiGongContainerTransform.childCount; i++)
        {
            _enemyNeiGongViews[i] = EnemyNeiGongContainerTransform.GetChild(i).GetComponent<StageNeiGongView>();
            _enemyNeiGongViews[i].Configure(new IndexPath("GetEnemyStageNeiGong", i));
        }

        _enemyWaiGongViews = new StageWaiGongView[StageManager.Instance.GetEnemyWaiGongCount()];
        for (int i = 0; i < EnemyWaiGongContainerTransform.childCount; i++)
        {
            _enemyWaiGongViews[i] = EnemyWaiGongContainerTransform.GetChild(i).GetComponent<StageWaiGongView>();
            _enemyWaiGongViews[i].Configure(new IndexPath("GetEnemyStageWaiGong", i));
        }
    }

    public void Refresh()
    {
        foreach(var view in _heroNeiGongViews) view.Refresh();
        foreach(var view in _heroWaiGongViews) view.Refresh();
        foreach(var view in _enemyNeiGongViews) view.Refresh();
        foreach(var view in _enemyWaiGongViews) view.Refresh();
    }

    public void SetHeroHealth(int value)
    {
        _heroHealthText.text = $"生命：{value}";
    }

    public void SetHeroArmor(int value)
    {
        _heroArmorText.text = $"护甲：{value}";
    }

    public void SetEnemyHealth(int value)
    {
        _enemyHealthText.text = $"生命：{value}";
    }

    public void SetEnemyArmor(int value)
    {
        _enemyArmorText.text = $"护甲：{value}";
    }
}
