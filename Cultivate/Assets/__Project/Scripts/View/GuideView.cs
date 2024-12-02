
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GuideView : MonoBehaviour
{
    [SerializeField] private TMP_Text _comment;
    [SerializeField] private RectTransform _dragCursor;
    [SerializeField] private RectTransform _clickCursor;
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform ConfirmAnchor;
    [SerializeField] private RectTransform BattleButtonAnchor;
    [SerializeField] private GameObject _clickHint;

    private Tween _handle;
    
    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();
    public void SetAddress(Address address)
    {
        _address = address;
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
            _dragCursor.gameObject.SetActive(false);
            _clickCursor.gameObject.SetActive(true);
            _clickCursor.position = ConfirmAnchor.position;
            SetCanClick(true);
            
            _handle?.Kill();
            _handle = TweenAnimation.Beats(_clickCursor);
            _handle.SetAutoKill().Restart();
        }
        else if (guide is EquipGuide equipGuide)
        {
            _comment.text = guide.GetComment();
            _dragCursor.gameObject.SetActive(true);
            _clickCursor.gameObject.SetActive(false);
            SetCanClick(false);

            bool found = equipGuide.GetFlowOfIndices(out DeckIndex[] result);
            if (!found)
            {
                _dragCursor.gameObject.SetActive(false);
                return;
            }
            
            _handle?.Kill();
            _dragCursor.localScale = Vector3.one;
            GuideAnimation anim = new GuideAnimation(_dragCursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.SkillItemFromDeckIndex(result[0])?.GetDisplayTransform(),
                CanvasManager.Instance.RunCanvas.DeckPanel.SkillItemFromDeckIndex(result[1])?.GetDisplayTransform());
            _handle = DOTween.Sequence()
                .Append(anim.GetHandle())
                .AppendInterval(0.4f)
                .SetLoops(-1, loopType: LoopType.Restart);
            _handle.SetAutoKill().Restart();
        }
        else if (guide is UnequipGuide unequipGuide)
        {
            _comment.text = guide.GetComment();
            _dragCursor.gameObject.SetActive(true);
            _clickCursor.gameObject.SetActive(false);
            SetCanClick(false);
            
            bool complete = unequipGuide.CheckComplete(out DeckIndex from);
            if (complete)
            {
                _dragCursor.gameObject.SetActive(false);
                return;
            }
            
            _handle?.Kill();
            _dragCursor.localScale = Vector3.one;
            GuideAnimation anim = new GuideAnimation(_dragCursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.SkillItemFromDeckIndex(from)?.GetDisplayTransform(),
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
            _dragCursor.gameObject.SetActive(true);
            _clickCursor.gameObject.SetActive(false);
            SetCanClick(false);

            bool complete = mergeGuide.CheckComplete(out DeckIndex[] result);
            if (complete)
            {
                _dragCursor.gameObject.SetActive(false);
                return;
            }
            
            _handle?.Kill();
            _dragCursor.localScale = Vector3.one;
            GuideAnimation anim = new GuideAnimation(_dragCursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.SkillItemFromDeckIndex(result[0])?.GetDisplayTransform(),
                CanvasManager.Instance.RunCanvas.DeckPanel.SkillItemFromDeckIndex(result[1])?.GetDisplayTransform());
            _handle = DOTween.Sequence()
                .Append(anim.GetHandle())
                .AppendInterval(0.4f)
                .SetLoops(-1, loopType: LoopType.Restart);
            _handle.SetAutoKill().Restart();
        }
        else if (guide is ClickBattleGuide clickBattleGuide)
        {
            _comment.text = guide.GetComment();
            _dragCursor.gameObject.SetActive(false);
            _clickCursor.gameObject.SetActive(true);
            _clickCursor.position = BattleButtonAnchor.position;
            SetCanClick(false);
            
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

            // _clickCursor.position = CanvasManager.Instance.RunCanvas.BattlePanel.CombatButton._rectTransform.position;
            
            _handle?.Kill();
            _handle = TweenAnimation.Beats(_clickCursor);
            _handle.SetAutoKill().Restart();
        }
        else
        {
            _handle?.Kill();
        }
    }

    private void SetCanClick(bool canClick)
    {
        _button.onClick.RemoveAllListeners();
        _clickHint.SetActive(canClick);
        if (canClick)
            _button.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        RunEnvironment env = RunManager.Instance.Environment;
        env.ReceiveSignalProcedure(new ConfirmGuideSignal());
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
