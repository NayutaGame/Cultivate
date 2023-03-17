using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeView : ItemView
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

        NameText.text = $"{runNode.GetName()}";
    }
}
