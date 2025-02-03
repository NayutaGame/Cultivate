
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanel : Panel
{
    [SerializeField] private CanvasGroup CoreCanvasGroup;
    [SerializeField] private Image DarkCurtainImage;
    [SerializeField] private Button DarkCurtainButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        DarkCurtainButton.onClick.RemoveAllListeners();
        DarkCurtainButton.onClick.AddListener(Return);
    }

    public virtual void Return()
    {
        
    }

    public override Tween EnterIdle()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Join(GetRect().DOScale(1f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CoreCanvasGroup.DOFade(1f, 0.15f))
            .Join(DarkCurtainImage.DOFade(0.7f, 0.15f));
    }

    public override Tween EnterHide()
    {
        return DOTween.Sequence()
            .Join(GetRect().DOScale(1.4f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CoreCanvasGroup.DOFade(0f, 0.15f))
            .Join(DarkCurtainImage.DOFade(0f, 0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
