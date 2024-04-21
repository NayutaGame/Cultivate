
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GuideView : MonoBehaviour
{
    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();
    public void SetAddress(Address address)
    {
        _address = address;
    }

    [SerializeField] private TMP_Text _comment;
    [SerializeField] private RectTransform _cursor;

    private Tween _handle;
    
    public void Refresh()
    {
        Guide guide = Get<Guide>();
        
        bool showing = guide != null;
        gameObject.SetActive(showing);
        if (!showing)
        {
            _handle?.Kill();
            return;
        }

        if (guide is EquipGuide equipGuide)
        {
            _comment.text = equipGuide.GetComment();

            bool complete = equipGuide.CheckComplete(out DeckIndex[] result);
            if (complete)
                return;

            Address start = result[0].ToAddress();
            Address end = result[1].ToAddress();
            
            _handle?.Kill();
            _cursor.localScale = Vector3.one;
            GuideAnimation anim = new GuideAnimation(_cursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(start),
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(end));
            _handle = DOTween.Sequence()
                .Append(anim.GetHandle())
                .AppendInterval(0.4f)
                .SetLoops(-1, loopType: LoopType.Restart);
            _handle.SetAutoKill().Restart();
        }
        else if (guide is UnequipGuide unequipGuide)
        {
            _comment.text = unequipGuide.GetComment();
            
            bool complete = unequipGuide.CheckComplete(out DeckIndex from);
            if (complete)
                return;
            
            Address start = from.ToAddress();
            
            _handle?.Kill();
            _cursor.localScale = Vector3.one;
            GuideAnimation anim = new GuideAnimation(_cursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(start),
                CanvasManager.Instance.RunCanvas.DeckPanel.DropRectTransform);
            _handle = DOTween.Sequence()
                .Append(anim.GetHandle())
                .AppendInterval(0.4f)
                .SetLoops(-1, loopType: LoopType.Restart);
            _handle.SetAutoKill().Restart();
        }
        else if (guide is MergeGuide mergeGuide)
        {
            _comment.text = mergeGuide.GetComment();

            bool complete = mergeGuide.CheckComplete(out DeckIndex[] result);
            if (complete)
                return;

            Address start = result[0].ToAddress();
            Address end = result[1].ToAddress();
            
            _handle?.Kill();
            _cursor.localScale = Vector3.one;
            GuideAnimation anim = new GuideAnimation(_cursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(start),
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(end));
            _handle = DOTween.Sequence()
                .Append(anim.GetHandle())
                .AppendInterval(0.4f)
                .SetLoops(-1, loopType: LoopType.Restart);
            _handle.SetAutoKill().Restart();
        }
        else if (guide is ClickBattleGuide clickBattleGuide)
        {
            _comment.text = clickBattleGuide.GetComment();
            Vector2? position = clickBattleGuide.GetPosition();
            if (position.HasValue)
            {
                _cursor.position = position.Value;
            }
            else
            {
                RectTransform rt = _comment.GetComponent<RectTransform>();
                Rect rect = rt.rect;
                _cursor.position = rt.position + new Vector3(rect.xMax, rect.yMin);
            }
            
            _handle?.Kill();
            _handle = DOTween.Sequence()
                .Append(_cursor.DOScale(0.8f, 0.1f).SetEase(Ease.InQuad))
                .Append(_cursor.DOScale(1.5f, 0.1f).SetEase(Ease.Linear))
                .Append(_cursor.DOScale(1f, 0.2f).SetEase(Ease.OutQuad))
                .AppendInterval(0.6f)
                .SetLoops(-1, loopType: LoopType.Restart);
            _handle.SetAutoKill().Restart();
        }
        else
        {
            _handle?.Kill();
        }
    }
}
