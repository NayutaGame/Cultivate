using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageCanvas : Singleton<StageCanvas>
{
    [SerializeField] private StageSkillPreview Preview;

    public TMP_Text SpeedText;
    public Slider SpeedSlider;
    public Button SkipButton;

    public TMP_Text _heroHealthText;
    public TMP_Text _heroArmorText;
    public TMP_Text _enemyHealthText;
    public TMP_Text _enemyArmorText;

    public TimelineView TimelineView;

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
        TimelineView.Configure();

        _heroBuffViews = new List<BuffView>();
        _enemyBuffViews = new List<BuffView>();

        SpeedSlider.onValueChanged.AddListener(SpeedChanged);
        SkipButton.onClick.AddListener(Skip);
    }

    private void SpeedChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        float speed;
        if (intValue == -1)
        {
            speed = 0.5f;
            SpeedText.text = "0.5倍速";
        }
        else
        {
            speed = Mathf.Pow(2, intValue);
            SpeedText.text = $"{Mathf.RoundToInt(speed)}倍速";
        }

        StageManager.Instance.SetSpeed(speed);
    }

    private void Skip()
    {
        StageManager.Instance.Skip();
    }

    public void InitialSetup()
    {
        TimelineView.InitialSetup();

        SpeedSlider.value = 0;
    }

    public void Refresh()
    {
        StageEntity home = StageManager.Instance.CurrEnv.Entities[0];

        SetHeroHealth(home.Hp);
        SetHeroArmor(home.Armor);

        StageEntity away = StageManager.Instance.CurrEnv.Entities[1];

        SetEnemyHealth(away.Hp);
        SetEnemyArmor(away.Armor);

        PopulateHeroBuffViews();
        PopulateEnemyBuffViews();

        foreach(var view in _heroBuffViews) view.Refresh();
        foreach(var view in _enemyBuffViews) view.Refresh();
    }

    private void PopulateHeroBuffViews()
    {
        int current = HeroBuffContainerTransform.childCount;
        int need = StageManager.Instance.CurrEnv.GetHeroBuffCount();

        (need, _) = Numeric.Negate(need, current);

        if (need <= 0) return;

        int length = HeroBuffContainerTransform.childCount;

        for (int i = length; i < need + length; i++)
        {
            BuffView v = Instantiate(BuffViewPrefab, HeroBuffContainerTransform).GetComponent<BuffView>();
            _heroBuffViews.Add(v);
            v.Configure(new IndexPath($"CurrEnv.Home.Buffs#{i}"));
        }
    }

    private void PopulateEnemyBuffViews()
    {
        int current = EnemyBuffContainerTransform.childCount;
        int need = StageManager.Instance.CurrEnv.GetEnemyBuffCount();

        (need, _) = Numeric.Negate(need, current);

        if (need <= 0) return;

        int length = EnemyBuffContainerTransform.childCount;

        for (int i = length; i < need + length; i++)
        {
            BuffView v = Instantiate(BuffViewPrefab, EnemyBuffContainerTransform).GetComponent<BuffView>();
            _enemyBuffViews.Add(v);
            v.Configure(new IndexPath($"CurrEnv.Away.Buffs#{i}"));
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

    public void SetIndexPathForPreview(IndexPath indexPath)
    {
        Preview.Configure(indexPath);
        Preview.Refresh();
    }

    public void UpdateMousePosForPreview(Vector2 pos)
    {
        Preview.UpdateMousePos(pos);
        Preview.Refresh();
    }
}
