
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileView : AddressBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private Image SelectionImage;

    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile p = Get<CharacterProfile>();
        Image.color = p.IsUnlocked() ? Color.white : Color.gray;
        NameText.text = p.GetEntry().Name;
    }

    // move to interact behaviour
    private Tween _animationHandle;

    public void SetSelected(bool selected)
    {
        _animationHandle?.Kill();
        _animationHandle = SelectionImage.DOFade(selected ? 1 : 0, 0.15f);
        _animationHandle.SetAutoKill().Restart();
    }
}
