using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeView : ItemView, IPointerClickHandler
{
    private Image Image;
    public TMP_Text NameText;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
        Image = GetComponent<Image>();
    }

    public override void Refresh()
    {
        base.Refresh();

        RunNode runNode = RunManager.Get<RunNode>(GetIndexPath());

        gameObject.SetActive(runNode != null);
        if (runNode == null)
            return;

        NameText.text = $"{runNode.GetTitle()}";
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
        RunManager.Instance.TryClickNode(GetIndexPath());
        RunCanvas.Instance.Refresh();
    }
}
