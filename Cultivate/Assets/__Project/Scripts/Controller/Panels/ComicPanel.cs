
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
        ImageButton.onClick.RemoveAllListeners();
        ImageButton.onClick.AddListener(ClickedSignal);
        ComicPanelDescriptor panelDescriptor = _address.Get<ComicPanelDescriptor>();
        SetPrefabEntry(panelDescriptor._prefabEntry);
        ComicView.Init();
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

    private Tween SelfTransitionTween()
        => DOTween.Sequence().AppendCallback(SelfTransition);

    private void SelfTransition()
    {
        _address = new Address("Run.Environment.ActivePanel");
        ImageButton.onClick.RemoveAllListeners();
        ImageButton.onClick.AddListener(ClickedSignal);
        ComicPanelDescriptor panelDescriptor = _address.Get<ComicPanelDescriptor>();
        SetPrefabEntry(panelDescriptor._prefabEntry);
        ComicView.Init();
    }

    private void ClickedSignal()
    {
        bool finished = ComicView.Click();
        if (!finished)
            return;
        
        ImageButton.onClick.RemoveAllListeners();
        RunManager.Instance.Environment.ReceiveSignalProcedure(new FinishedComicSignal());
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
    }
}
