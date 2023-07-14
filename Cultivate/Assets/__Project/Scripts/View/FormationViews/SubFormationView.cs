using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubFormationView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text ConditionText;
    [SerializeField] private TMP_Text RewardText;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh()
    {
    }
}
