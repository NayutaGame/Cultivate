using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FormationView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private SubFormationView SubFormationViews;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh()
    {
    }
}
