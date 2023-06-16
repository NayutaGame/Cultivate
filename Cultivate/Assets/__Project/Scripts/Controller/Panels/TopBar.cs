using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public TMP_Text MingYuanText;
    public TMP_Text GoldText;
    public TMP_Text HealthText;
    public TMP_Text JingJieText;

    public Button MapButton;
    public Button SystemButton;

    public void Configure()
    {
        // MapButton.onClick.AddListener(MapButton);
    }

    public void Refresh()
    {
        IEntityModel entity = RunManager.Instance.Battle.Hero;

        MingYuanText.text = RunManager.Instance.MingYuan.ToString();
        GoldText.text = RunManager.Instance.XiuWei.ToString();
        HealthText.text = entity.GetFinalHealth().ToString();
        JingJieText.text = $"{entity.GetJingJie().ToString()}期";
    }
}
