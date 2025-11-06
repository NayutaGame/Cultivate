
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class NarrativePanel : Panel
{
    [SerializeField] private TMP_Text LeftCharacterName;
    [SerializeField] private TMP_Text RightCharacterName;
    [SerializeField] private TMP_Text NarrativeText;

    [SerializeField] private PropagateClick ClickReceiver;

    private Address _address;
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        ClickReceiver._onPointerClick = ReceiveClick;
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.CommendProcessedNeuron.Add(CommendProcessed);
        RunManager.Instance.Environment.NarrativeTextChangedNeuron.Add(NarrativeTextChangedStaging);
        RunManager.Instance.Environment.CharacterNameChangedNeuron.Add(CharacterNameChangedStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.CommendProcessedNeuron.Remove(CommendProcessed);
        RunManager.Instance.Environment.NarrativeTextChangedNeuron.Remove(NarrativeTextChangedStaging);
        RunManager.Instance.Environment.CharacterNameChangedNeuron.Remove(CharacterNameChangedStaging);
    }

    public override void Refresh()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        NarrativeCell cell = cellAdapter.AsCell() as NarrativeCell;
    }

    public void QueueProcessNarrativeSignal()
    {
        CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueSignal(new ProcessNarrativeSignal());
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0))
            .AppendCallback(QueueProcessNarrativeSignal);

    public override Tween EnterHide()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(false));

    public void CommendProcessed(Commend commend)
    {
        switch (commend.GetProceedMode()) {
            case ProceedMode.Instant:
                QueueProcessNarrativeSignal();
                break;
            case ProceedMode.Wait200ms:
                CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueInterval(200);
                QueueProcessNarrativeSignal();
                break;
            case ProceedMode.WaitSignal:
                break;
        }
    }

    public void NarrativeTextChangedStaging(string narrativeText)
    {
        NarrativeText.text = narrativeText;
    }

    public void CharacterNameChangedStaging(string characterName, bool isHome)
    {
        if (isHome)
            LeftCharacterName.text = characterName;
        else
            RightCharacterName.text = characterName;
    }

    private void ReceiveClick(PointerEventData d)
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        NarrativeCell cell = cellAdapter.AsCell() as NarrativeCell;

        AnimationQueue queue = CanvasManager.Instance.RunCanvas.GetAnimationQueue();
        
        bool isPlaying = queue.IsPlaying;
        if (isPlaying)
        {
            bool isSkippable = false;
            if (isSkippable)
            {
                queue.CompleteCurrAnimation();
                if (!queue.IsPlaying)
                {
                    QueueProcessNarrativeSignal();
                }
            }
        }
        else
        {
            QueueProcessNarrativeSignal();
        }
    }
}