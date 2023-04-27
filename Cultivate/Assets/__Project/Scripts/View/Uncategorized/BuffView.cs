using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffView : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text StackText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public virtual void Refresh()
    {
        // TryGetHeroBuff
        Buff b = StageManager.Get<Buff>(IndexPath);

        gameObject.SetActive(b != null);
        if (b == null) return;

        NameText.text = $"{b.GetName()}";
        StackText.text = $"{b.Stack}";
    }
}
