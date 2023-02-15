using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageNeiGongView : MonoBehaviour
{
    public TMP_Text InfoText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        Refresh();
    }

    public void Refresh()
    {
        StageNeiGong chip = StageManager.Get<StageNeiGong>(IndexPath);
        InfoText.text = $"{chip.GetName()}[{chip.Level}]";
    }
}
