using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeView : MonoBehaviour, IIndexPath, IPointerClickHandler
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private Image Image;
    public TMP_Text NameText;

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        Image = GetComponent<Image>();
    }

    public virtual void Refresh()
    {
        RunNode runNode = RunManager.Get<RunNode>(GetIndexPath());

        gameObject.SetActive(runNode != null);
        if (runNode == null)
            return;

        if (NameText != null)
            NameText.text = runNode.GetTitle();

        if (Image != null)
            Image.sprite = runNode.Sprite;

        switch (runNode.State)
        {
            case RunNode.RunNodeState.Current:
                Image.color = Color.cyan;
                break;
            case RunNode.RunNodeState.Locked:
                Image.color = Color.gray;
                break;
            case RunNode.RunNodeState.Missed:
                Image.color = Color.red;
                break;
            case RunNode.RunNodeState.Passed:
                Image.color = Color.yellow;
                break;
            case RunNode.RunNodeState.ToChoose:
                Image.color = Color.green;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TryClickNode(GetIndexPath());
        RunCanvas.Instance.Refresh();
    }

    public bool TryClickNode(IndexPath indexPath)
    {
        RunNode runNode = RunManager.Get<RunNode>(indexPath);
        if (runNode.State != RunNode.RunNodeState.ToChoose || !RunManager.Instance.Map.Selecting)
            return false;

        PanelDescriptor panelDescriptor = RunManager.Instance.Map.SelectedNode(runNode);
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        return true;
    }
}
