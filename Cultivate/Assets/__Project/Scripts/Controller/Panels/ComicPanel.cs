
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ComicPanel : Panel
{
    [SerializeField] private Button ImageButton;

    private Address _address;
    
    public Transform Anchor;
    private PrefabEntry PrefabEntry;
    private GameObject ComicGameObject;
    private ComicView ComicView;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Comic Panel");
        animator[0, 1] = EnterIdle;
        animator[1, 1] = SelfTransitionTween;
        animator[-1, 0] = HideTweenWithCurtain;
        
        animator.SetState(0);
        return animator;
    }
    
    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(SetPrefabEntry)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE));

    private Tween SelfTransitionTween()
        => DOTween.Sequence().AppendCallback(SetPrefabEntry);

    private void ClickedSignal()
    {
        bool finished = ComicView.Click();
        if (!finished)
            return;
        
        ImageButton.onClick.RemoveAllListeners();
        RunManager.Instance.Environment.ReceiveSignalProcedure(new FinishedComicSignal());
    }

    private void SetPrefabEntry()
    {
        ImageButton.onClick.RemoveAllListeners();
        ImageButton.onClick.AddListener(ClickedSignal);
        ComicCell cell = _address.Get<ComicCell>();
        SetPrefabEntry(cell._prefabEntry);
    }
    
    private void SetPrefabEntry(PrefabEntry targetPrefabEntry)
    {
        if (PrefabEntry == targetPrefabEntry)
            return;
        
        if (ComicGameObject != null)
            Destroy(ComicGameObject);

        PrefabEntry = targetPrefabEntry;
        ComicGameObject = Instantiate(PrefabEntry.Prefab, Anchor);
        ComicView = ComicGameObject.GetComponent<ComicView>();
        ComicView.Init();
    }
}
