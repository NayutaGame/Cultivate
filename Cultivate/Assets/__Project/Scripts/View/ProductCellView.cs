using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductCellView : MonoBehaviour, IIndexPath
{
    // public TMP_Text NameText;
    // public TMP_Text CostText;
    // public GameObject Button;
    // public GameObject Drag;
    //
    // private IndexPath _indexPath;
    // public IndexPath GetIndexPath() => _indexPath;
    //
    // private bool _canAfford;
    //
    // public void Configure(IndexPath indexPath)
    // {
    //     _indexPath = indexPath;
    //
    //     PropagateDrag drag = Drag.GetComponent<PropagateDrag>();
    //     drag._indexPath = _indexPath;
    //     drag._onPointerDown = OnPointerDown;
    //     drag._onBeginDrag = OnBeginDrag;
    //     drag._onEndDrag = OnEndDrag;
    //     drag._onDrag = OnDrag;
    //
    //     Button.GetComponent<Button>().onClick.AddListener(Click);
    // }
    //
    // public virtual void Refresh()
    // {
    //     Product product = RunManager.Get<Product>(_indexPath);
    //
    //     gameObject.SetActive(product != null);
    //     if (product == null) return;
    //
    //     NameText.text = product.GetName();
    //     CostText.text = product.GetCost().ToString();
    //
    //     Button.SetActive(product.IsClick());
    //     Drag.SetActive(product.IsDrag());
    //
    //     _canAfford = RunManager.Instance.CanAfford(product);
    //     Button.GetComponent<Button>().interactable = _canAfford;
    //     if (_canAfford)
    //     {
    //         NameText.color = Color.black;
    //     }
    //     else
    //     {
    //         NameText.color = Color.red;
    //     }
    // }
    //
    // private void Click()
    // {
    //     RunManager.Instance.TryClickProduct(_indexPath);
    //     RunCanvas.Instance.Refresh();
    // }
    //
    // public void OnPointerDown(PointerEventData eventData) { }
    //
    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     if (!_canAfford)
    //     {
    //         eventData.pointerDrag = null;
    //         return;
    //     }
    //     RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragProductCell(this);
    //
    //     RunCanvas.Instance.GhostProduct.Configure(GetIndexPath());
    //     RunCanvas.Instance.GhostProduct.Refresh();
    //     RunCanvas.Instance.Refresh();
    // }
    //
    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    //
    //     RunCanvas.Instance.GhostProduct.Configure(null);
    //     RunCanvas.Instance.GhostProduct.Refresh();
    //     RunCanvas.Instance.Refresh();
    // }
    //
    // public void OnDrag(PointerEventData eventData)
    // {
    //     RunCanvas.Instance.GhostProduct.UpdateMousePos(eventData.position);
    //     // when pointing a tile, outcome preview
    // }
    public IndexPath GetIndexPath()
    {
        throw new System.NotImplementedException();
    }
}
