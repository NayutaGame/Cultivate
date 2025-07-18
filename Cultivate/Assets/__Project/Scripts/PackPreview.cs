
using UnityEngine;
using UnityEngine.EventSystems;

public class PackPreview : XView
{
    [SerializeField] private PropagateClick _propagateClick;
    [SerializeField] private ListView ListView;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _propagateClick._onPointerClick = ClosePreview;
        ListView.SetAddress("InventoryFromExpandedPack");
    }

    public void ClosePreview()
        => gameObject.SetActive(false);

    public void ClosePreview(PointerEventData d)
        => gameObject.SetActive(false);
}
