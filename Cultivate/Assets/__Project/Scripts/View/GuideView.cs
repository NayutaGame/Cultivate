
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideView : MonoBehaviour
{
    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();
    public void SetAddress(Address address)
    {
        _address = address;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ButtonClick);
    }

    [SerializeField] private TMP_Text _comment;
    [SerializeField] private RectTransform _cursor;
    [SerializeField] private Button _button;

    private Tween _handle;

    private void ButtonClick()
    {
        RunEnvironment env = RunManager.Instance.Environment;
        env.ReceiveSignalProcedure(new ConfirmGuideSignal());
        CanvasManager.Instance.RunCanvas.Refresh();
    }
    
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

        if (guide is ConfirmGuide confirmGuide)
        {
            _comment.text = guide.GetComment();
            _cursor.gameObject.SetActive(false);
            _button.gameObject.SetActive(true);
        }
        else if (guide is EquipGuide equipGuide)
        {
            _comment.text = guide.GetComment();
            _cursor.gameObject.SetActive(true);
            _button.gameObject.SetActive(false);

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
            _comment.text = guide.GetComment();
            _cursor.gameObject.SetActive(true);
            _button.gameObject.SetActive(false);
            
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
            _comment.text = guide.GetComment();
            _cursor.gameObject.SetActive(true);
            _button.gameObject.SetActive(false);

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
            _comment.text = guide.GetComment();
            _cursor.gameObject.SetActive(true);
            _button.gameObject.SetActive(false);
            
            // Vector2? position = clickBattleGuide.GetPosition();
            // if (position.HasValue)
            // {
            //     _cursor.position = position.Value;
            // }
            // else
            // {
            //     RectTransform rt = _comment.GetComponent<RectTransform>();
            //     Rect rect = rt.rect;
            //     _cursor.position = rt.position + new Vector3(rect.xMax, rect.yMin);
            // }

            _cursor.position = CanvasManager.Instance.RunCanvas.BattlePanel.CombatButton._rectTransform.position;
            
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
