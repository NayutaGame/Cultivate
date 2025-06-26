
using UnityEngine;
using UnityEngine.EventSystems;

public class PackPreview : XView
{
    [SerializeField] private PropagateClick _propagateClick;
    [SerializeField] private ListView _listView;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _propagateClick._onPointerClick = ClosePreview;
        _listView.SetAddress("InventoryFromExpandedPack");
    }

    public void Sync()
        => _listView.Sync();

    public void ClosePreview()
        => gameObject.SetActive(false);

    public void ClosePreview(PointerEventData d)
        => gameObject.SetActive(false);
}
