
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimelineView : MonoBehaviour
{
    public RectTransform[] AwaySlots;
    public RectTransform[] HomeSlots;

    private static readonly int CurrSlot = 3;

    public RectTransform NoteContainer;
    public GameObject NotePrefab;

    private List<NoteView> _views;
    public List<NoteView> Views => _views;

    private int _time;

    public void Configure()
    {
        _views = new List<NoteView>();
    }

    public async Task Play()
    {
        InitialSetup();

        // opening animation
        for (int i = 0; i < 20; i++)
        {
            await ShiftAnimation();
            await NoteAnimation();
        }
    }

    public void InitialSetup()
    {
        _time = -1;
        List<StageNote> batch = StageManager.Instance.EndEnv.Report.Timeline.GetNotes(0, 8);

        for (int i = 0; i < batch.Count; i++)
        {
            StageNote note = batch[i];
            RectTransform slot = note.IsHome ? HomeSlots[i + 4] : AwaySlots[i + 4];
            NoteView v = Instantiate(NotePrefab, slot.position, slot.rotation, NoteContainer).GetComponent<NoteView>();
            _views.Add(v);
            v.Configure(new IndexPath($"EndEnv.Report.Timeline.Notes#{note.TemporalIndex}"));
        }
    }

    public async Task ShiftAnimation()
    {
        Sequence seq = DOTween.Sequence().SetAutoKill();

        int currTime = _time;
        int nextTime = _time + 1;

        for (int i = 0; i < _views.Count; i++)
        {
            NoteView v = _views[i];
            StageNote note = StageManager.Get<StageNote>(v.GetIndexPath());
            int spatialIndex = note.TemporalIndex - nextTime;

            if (spatialIndex == -4) // destruction
            {
                NoteView toDestroy = _views[i];
                _views.RemoveAt(i);
                Destroy(toDestroy.gameObject);
                i--;
                continue;
            }

            RectTransform nextSlot = note.IsHome ? HomeSlots[spatialIndex + 3] : AwaySlots[spatialIndex + 3];
            Vector3 nextPos = nextSlot.position;

            if (spatialIndex == -1) // shrink
            {
                seq.Join(_views[i].transform.DOMove(nextPos, 0.6f).SetEase(Ease.InOutQuad));
                seq.Join(_views[i].Shrink());
            }
            else if (spatialIndex == 0) // expand
            {
                seq.Join(_views[i].transform.DOMove(nextPos, 0.6f).SetEase(Ease.InOutQuad));
                seq.Join(_views[i].Expand());
            }
            else // shift
            {
                seq.Join(_views[i].transform.DOMove(nextPos, 0.6f).SetEase(Ease.InOutQuad));
            }
        }

        // creation
        seq.AppendCallback(TryCreate);
        seq.AppendInterval(0.2f);

        _time += 1;

        seq.Restart();

        await seq.AsyncWaitForCompletion();
    }

    public async Task NoteAnimation()
    {
        await StageManager.Instance.PlayTween();

        // bullet time at killing moment
        // gradually accelerating
        // pause, speed control and skip
        // camera shake when large attacks
    }

    private void TryCreate()
    {
        StageNote toCreate = StageManager.Instance.EndEnv.Report.Timeline.GetNote(_time + 8);

        RectTransform slot = toCreate.IsHome ? HomeSlots[^1] : AwaySlots[^1];
        NoteView v = Instantiate(NotePrefab, slot.position, slot.rotation, NoteContainer).GetComponent<NoteView>();
        _views.Add(v);
        v.Configure(new IndexPath($"EndEnv.Report.Timeline.Notes#{toCreate.TemporalIndex}"));
    }
}
