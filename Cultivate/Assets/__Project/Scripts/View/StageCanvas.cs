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

    public Transform HeroBuffContainerTransform;
    public Transform EnemyBuffContainerTransform;
    private List<BuffView> _heroBuffViews;
    private List<BuffView> _enemyBuffViews;

    public GameObject BuffViewPrefab;

    public override void DidAwake()
    {
        base.DidAwake();
        Configure();
    }

    public void Configure()
    {
        // StageManager.Instance.GetHeroNeiGongCount()
        // StageManager.Instance.GetHeroWaiGongCount()
        _heroNeiGongViews = new StageNeiGongView[RunManager.NeiGongLimit];
        for (int i = 0; i < HeroNeiGongContainerTransform.childCount; i++)
        {
            _heroNeiGongViews[i] = HeroNeiGongContainerTransform.GetChild(i).GetComponent<StageNeiGongView>();
            _heroNeiGongViews[i].Configure(new IndexPath($"Home.NeiGongs#{i}"));
        }

        _heroWaiGongViews = new StageWaiGongView[RunManager.WaiGongLimit];
        for (int i = 0; i < HeroWaiGongContainerTransform.childCount; i++)
        {
            _heroWaiGongViews[i] = HeroWaiGongContainerTransform.GetChild(i).GetComponent<StageWaiGongView>();
            _heroWaiGongViews[i].Configure(new IndexPath($"Home.WaiGongs#{i}"));
        }

        _enemyNeiGongViews = new StageNeiGongView[RunManager.NeiGongLimit];
        for (int i = 0; i < EnemyNeiGongContainerTransform.childCount; i++)
        {
            _enemyNeiGongViews[i] = EnemyNeiGongContainerTransform.GetChild(i).GetComponent<StageNeiGongView>();
            _enemyNeiGongViews[i].Configure(new IndexPath($"Away.NeiGongs#{i}"));
        }

        _enemyWaiGongViews = new StageWaiGongView[RunManager.WaiGongLimit];
        for (int i = 0; i < EnemyWaiGongContainerTransform.childCount; i++)
        {
            _enemyWaiGongViews[i] = EnemyWaiGongContainerTransform.GetChild(i).GetComponent<StageWaiGongView>();
            _enemyWaiGongViews[i].Configure(new IndexPath($"Away.WaiGongs#{i}"));
        }

        _heroBuffViews = new List<BuffView>();
        _enemyBuffViews = new List<BuffView>();
    }

    public void Refresh()
    {
        foreach(var view in _heroNeiGongViews) view.Refresh();
        foreach(var view in _heroWaiGongViews) view.Refresh();
        foreach(var view in _enemyNeiGongViews) view.Refresh();
        foreach(var view in _enemyWaiGongViews) view.Refresh();

        PopulateHeroBuffViews();
        PopulateEnemyBuffViews();

        foreach(var view in _heroBuffViews) view.Refresh();
        foreach(var view in _enemyBuffViews) view.Refresh();
    }

    private void PopulateHeroBuffViews()
    {
        int current = HeroBuffContainerTransform.childCount;
        int need = StageManager.Instance.GetHeroBuffCount();

        (need, _) = Numeric.Negate(need, current);

        if (need <= 0) return;

        int length = HeroBuffContainerTransform.childCount;

        for (int i = length; i < need + length; i++)
        {
            BuffView v = Instantiate(BuffViewPrefab, HeroBuffContainerTransform).GetComponent<BuffView>();
            _heroBuffViews.Add(v);
            v.Configure(new IndexPath($"Home.Buffs#{i}"));
        }
    }

    private void PopulateEnemyBuffViews()
    {
        int current = EnemyBuffContainerTransform.childCount;
        int need = StageManager.Instance.GetEnemyBuffCount();

        (need, _) = Numeric.Negate(need, current);

        if (need <= 0) return;

        int length = EnemyBuffContainerTransform.childCount;

        for (int i = length; i < need + length; i++)
        {
            BuffView v = Instantiate(BuffViewPrefab, EnemyBuffContainerTransform).GetComponent<BuffView>();
            _heroBuffViews.Add(v);
            v.Configure(new IndexPath($"Away.Buffs#{i}"));
        }
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
