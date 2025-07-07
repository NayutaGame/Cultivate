
using UnityEngine;
using UnityEngine.UI;

public class UnlockIcon : XView
{
    [SerializeField] public CanvasGroup CanvasGroup;
    [SerializeField] public Image ContentImage;
    [SerializeField] public Image LockIcon;

    public override void Refresh()
    {
        base.Refresh();

        AchievementProfile p = Get<AchievementProfile>();
        ContentImage.sprite = p.GetEntry().GetSprite();
        LockIcon.gameObject.SetActive(p.IsUnlocked());
    }

    // 锁住
    //     已解锁
    //
    // 锁住的时候，需要一个进度
    //
    //     Hover
    // Annotation
    //
    //     Annotation中包含，Title Description
}
