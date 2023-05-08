using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageWaiGongView : MonoBehaviour
{
    public TMP_Text InfoText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh()
    {
        StageSkill chip = StageManager.Get<StageSkill>(IndexPath);

        // InfoText.text = chip == null ? "" : $"{chip.GetName()}[{chip.Level}]";
    }
}
