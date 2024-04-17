
using DG.Tweening;
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

    [SerializeField] private RectTransform _cursor;

    private Tween _handle;
    
    public void Refresh()
    {
        GuideDescriptor guideDescriptor = Get<GuideDescriptor>();

        bool showing = guideDescriptor != null;
        gameObject.SetActive(showing);
        if (!showing)
        {
            _handle?.Kill();
            return;
        }

        if (guideDescriptor is DragSkillToSlotGuideDescriptor drag)
        {
            Address start = drag.FindStart();
            Address end = drag.FindEnd();

            if (start.Equals(end) || end == null)
                return;
            
            _handle?.Kill();
            GuideAnimation anim = new GuideAnimation(_cursor,
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(start),
                CanvasManager.Instance.RunCanvas.DeckPanel.Find(end));
            _handle = DOTween.Sequence()
                .AppendInterval(0.2f)
                .Append(anim.GetHandle())
                .AppendInterval(0.2f)
                .SetLoops(-1, loopType: LoopType.Restart)
                .SetAutoKill();
            _handle.Restart();
        }
        else
        {
            _handle?.Kill();
        }
    }
}
