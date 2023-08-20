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

    public Button VolumeButton;

    public Image VolumeImage;
    public Image VolumeHoverImage;

    public Sprite AudibleSprite;
    public Sprite AudibleHoverSprite;
    public Sprite MutedSprite;
    public Sprite MutedHoverSprite;

    public Button SystemButton;

    public void Configure()
    {
        VolumeButton.onClick.RemoveAllListeners();
        VolumeButton.onClick.AddListener(ToggleAudible);
    }

    public void Refresh()
    {
        IEntityModel entity = RunManager.Instance.Battle.Hero;

        MingYuanText.text = RunManager.Instance.Battle.HeroMingYuan.ToString();
        GoldText.text = RunManager.Instance.XiuWei.ToString();
        HealthText.text = entity.GetFinalHealth().ToString();
        JingJieText.text = $"{entity.GetJingJie().ToString()}期";

        if (AudioManager.Instance.IsAudible)
        {
            VolumeImage.sprite = AudibleSprite;
            VolumeHoverImage.sprite = AudibleHoverSprite;
        }
        else
        {
            VolumeImage.sprite = MutedSprite;
            VolumeHoverImage.sprite = MutedHoverSprite;
        }
    }

    private void ToggleAudible()
    {
        AudioManager.Instance.ToggleAudible();
        Refresh();
    }
}
