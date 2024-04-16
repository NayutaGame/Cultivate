
using System;
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

    private Tweener _handle;
    
    public void Refresh()
    {
        GuideDescriptor guideDescriptor = Get<GuideDescriptor>();

        bool showing = guideDescriptor != null;
        gameObject.SetActive(showing);
        if (!showing)
        {
            _handle.Kill();
            return;
        }

        if (guideDescriptor is DragSkillToSlotGuideDescriptor drag)
        {
            Address start = drag.FindStart();
            Address end = drag.FindEnd();

            if (start.Equals(end) || end == null)
                return;
        
            RectTransform startView = CanvasManager.Instance.RunCanvas.DeckPanel.Find(start);
            RectTransform endView = CanvasManager.Instance.RunCanvas.DeckPanel.Find(end);
            
            _handle.Kill();
        
            _handle = _cursor.DOAnchorPos(endView.anchoredPosition, 0.5f)
                .From(startView.anchoredPosition)
                .SetLoops(-1, loopType: LoopType.Restart)
                .SetEase(Ease.InOutQuad).SetAutoKill();
            
            _handle.Restart();
        }
        
        _handle.Kill();
    }
}
