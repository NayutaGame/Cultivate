
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

    private static readonly Vector3 SHRINK_SCALE = Vector3.one * 0.5f;
    private static readonly Vector3 EXPAND_SCALE = Vector3.one * 1;

    [NonSerialized] public int TotalCount;
    [NonSerialized] public int FutureCount;
    [NonSerialized] public int PastCount;

    public RectTransform NoteContainer;
    public GameObject NotePrefab;

    private List<StageSkillView> _views;
    public List<StageSkillView> Views => _views;

    private int _time;

    public void Configure()
    {
        ClearViews();
        _views = new();
        foreach (var v in _views)
            ConfigureNeuron(v.GetInteractBehaviour());

        // TotalCount = HomeSlots.Length;
        FutureCount = HomeSlots.Length - (IndexOfCurr + 1);
        // PastCount = IndexOfCurr;
    }

    private void ConfigureNeuron(InteractBehaviour ib)
    {
        ib.PointerEnterNeuron.Join(StageManager.Instance.Pause);
        ib.PointerExitNeuron.Join(StageManager.Instance.Resume);
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
            StageSkillView v = Instantiate(NotePrefab, slot.position, slot.rotation, NoteContainer).GetComponent<StageSkillView>();
            _views.Add(v);
            v.CheckAwake();
            v.SetAddress(new Address($"Stage.Timeline.Notes#{note.TemporalIndex}"));
            v.TimelineScale.localScale = SHRINK_SCALE;
            ConfigureNeuron(v.GetInteractBehaviour());
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
            StageSkillView v = _views[i];
            StageNote note = v.Get<StageNote>();
            int spatialIndex = note.TemporalIndex - nextTime + 1 + IndexOfCurr;

            if (spatialIndex == 0)
            {
                StageSkillView toDestroy = _views[i];
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
                seq.Join(_views[i].TimelineScale.DOScale(SHRINK_SCALE, 0.6f).SetEase(Ease.InOutQuad));
            }
            else if (spatialIndex == IndexOfCurr + 1)
            {
                seq.Join(_views[i].TimelineScale.DOScale(EXPAND_SCALE, 0.6f).SetEase(Ease.InOutQuad));
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
        StageSkillView v = Instantiate(NotePrefab, slot.position, slot.rotation, NoteContainer).GetComponent<StageSkillView>();
        _views.Add(v);
        v.CheckAwake();
        v.SetAddress(new Address($"Stage.Timeline.Notes#{toCreate.TemporalIndex}"));
        v.TimelineScale.localScale = SHRINK_SCALE;
        ConfigureNeuron(v.GetInteractBehaviour());
        v.Refresh();
    }
}
