
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NarrativePanel : Panel
{
    [SerializeField] private TMP_Text LeftCharacterName;
    [SerializeField] private TMP_Text RightCharacterName;
    [SerializeField] private TMP_Text NarrativeText;

    [SerializeField] private PropagateClick ClickReceiver;

    private Address _address;

    [SerializeField] private SpineModelView LeftCharacter;
    [SerializeField] private SpineModelView RightCharacter;
    
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
        RunManager.Instance.Environment.CharacterChangedNeuron.Add(CharacterChangedStaging);
        ClearContext();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.CommendProcessedNeuron.Remove(CommendProcessed);
        RunManager.Instance.Environment.NarrativeTextChangedNeuron.Remove(NarrativeTextChangedStaging);
        RunManager.Instance.Environment.CharacterChangedNeuron.Remove(CharacterChangedStaging);
    }

    public override void Refresh()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        NarrativeCell cell = cellAdapter.AsCell() as NarrativeCell;
    }

    public void ClearContext()
    {
        
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

    public void CharacterChangedStaging(CharacterEntry characterEntry, bool isHome)
    {
        if (isHome)
        {
            LeftCharacter.SetPrefabEntry(characterEntry.GetNarrativePrefabEntry());
            LeftCharacterName.text = characterEntry.GetName();
        }
        else
        {
            RightCharacter.SetPrefabEntry(characterEntry.GetNarrativePrefabEntry());
            RightCharacterName.text = characterEntry.GetName();
        }
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