
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsTabView : SimpleView
{
    [SerializeField] public Image HoverImage;
    [SerializeField] private TMP_Text Text;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        GetInteractBehaviour().PointerEnterNeuron.Join(Hover);
        GetInteractBehaviour().PointerExitNeuron.Join(Unhover);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        SettingsTab settingsTab = Get<SettingsTab>();
        Text.text = settingsTab.Name;

        if (AppManager.Instance.Settings.IsSelectedTab(settingsTab))
            HoverImage.color = new Color(1, 1, 1, 0.4f);
    }

    private Tween _handle;

    private void SetAlpha(float alpha)
    {
        _handle?.Kill();
        _handle = HoverImage.DOFade(alpha, 0.15f).SetEase(Ease.OutQuad);
        _handle.SetAutoKill().Restart();
    }

    public void Select()
    {
        SetAlpha(0.4f);
    }

    public void Unselect()
    {
        SetAlpha(0);
    }
    
    public void Hover(InteractBehaviour ib, PointerEventData d)
    {
        SettingsTab settingsTab = Get<SettingsTab>();
        if (AppManager.Instance.Settings.IsSelectedTab(settingsTab))
            return;
        
        SetAlpha(0.1f);
    }

    public void Unhover(InteractBehaviour ib, PointerEventData d)
    {
        SettingsTab settingsTab = Get<SettingsTab>();
        if (AppManager.Instance.Settings.IsSelectedTab(settingsTab))
            return;
        
        SetAlpha(0f);
    }
}
