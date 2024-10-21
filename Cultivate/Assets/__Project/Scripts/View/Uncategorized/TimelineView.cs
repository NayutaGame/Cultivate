
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using Tween = DG.Tweening.Tween;

public class TimelineView : Singleton<TimelineView>
{
    public int IndexOfCurr;
    public RectTransform[] AwaySlots;
    public RectTransform[] HomeSlots;

    [NonSerialized] public int TotalCount;
    [NonSerialized] public int FutureCount;
    [NonSerialized] public int PastCount;

    public RectTransform NoteContainer;
    public GameObject NotePrefab;

    private List<StageSkillCardView> _views;
    public List<StageSkillCardView> Views => _views;

    private int _time;

    public void Configure()
    {
        ClearViews();
        _views = new List<StageSkillCardView>();
        foreach (var v in _views)
            ConfigureNeuron(v.GetComponent<InteractBehaviour>());

        // TotalCount = HomeSlots.Length;
        FutureCount = HomeSlots.Length - (IndexOfCurr + 1);
        // PastCount = IndexOfCurr;
    }

    private void ConfigureNeuron(InteractBehaviour stageSkillIb)
    {
        stageSkillIb.PointerEnterNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerEnter, StageManager.Instance.Pause);
        stageSkillIb.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit, StageManager.Instance.Resume);
        stageSkillIb.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);
    }

    public void InitialSetup()
    {
        ClearViews();

        _time = -1;
        List<StageNote> batch = StageManager.Instance.Timeline.GetNotes(0, FutureCount);

        for (int i = 0; i < batch.Count; i++)
        {
            StageNote note = batch[i];
            RectTransform slot = (note.IsHome ? HomeSlots : AwaySlots)[IndexOfCurr + 1 + i];
            StageSkillCardView v = Instantiate(NotePrefab, slot.position, slot.rotation, NoteContainer).GetComponent<StageSkillCardView>();
            v.transform.localScale = 0.5f * Vector3.one;
            _views.Add(v);
            v.SetAddress(new Address($"Stage.Timeline.Notes#{note.TemporalIndex}"));
            ConfigureNeuron(v.GetComponent<InteractBehaviour>());
            v.Refresh();
        }
    }

    private void ClearViews()
    {
        if (_views == null)
            return;

        foreach (var v in _views)
            Destroy(v.gameObject);
        _views.Clear();
    }

    public Tween ShiftAnimation()
    {
        Sequence seq = DOTween.Sequence();

        int currTime = _time;
        int nextTime = _time + 1;

        for (int i = 0; i < _views.Count; i++)
        {
            StageSkillCardView v = _views[i];
            StageNote note = v.Get<StageNote>();
            int spatialIndex = note.TemporalIndex - nextTime + 1 + IndexOfCurr;

            if (spatialIndex == 0)
            {
                StageSkillCardView toDestroy = _views[i];
                _views.RemoveAt(i);
                Destroy(toDestroy.gameObject);
                i--;
                continue;
            }

            int nextIndex = spatialIndex - 1;
            RectTransform nextSlot = (note.IsHome ? HomeSlots : AwaySlots)[nextIndex];
            Vector3 nextPos = nextSlot.position;

            seq.Join(_views[i].transform.DOMove(nextPos, 0.6f).SetEase(Ease.InOutQuad));

            if (spatialIndex == IndexOfCurr)
            {
                seq.Join(_views[i].GetShrinkTween());
            }
            else if (spatialIndex == IndexOfCurr + 1)
            {
                seq.Join(_views[i].GetExpandTween());
            }
        }

        // creation
        seq.AppendCallback(TryCreate);
        seq.AppendInterval(0.3f);

        _time += 1;

        return seq;
    }

    private void TryCreate()
    {
        StageNote toCreate = StageManager.Instance.Timeline.GetNote(_time + FutureCount);
        if (toCreate == null)
            return;

        RectTransform slot = toCreate.IsHome ? HomeSlots[^1] : AwaySlots[^1];
        StageSkillCardView v = Instantiate(NotePrefab, slot.position, slot.rotation, NoteContainer).GetComponent<StageSkillCardView>();
        v.transform.localScale = 0.5f * Vector3.one;
        _views.Add(v);
        v.SetAddress(new Address($"Stage.Timeline.Notes#{toCreate.TemporalIndex}"));
        ConfigureNeuron(v.GetComponent<InteractBehaviour>());
        v.Refresh();
    }
}
