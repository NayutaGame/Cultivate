using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsContentView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private SettingsContentModel _model;

    private void Awake()
    {
        if (_model == null)
            Configure(null);
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        // _model = _indexPath == null ? SettingsContentModel.Default : DataManager.Get<SettingsContentModel>(_indexPath);
        _model = DataManager.Get<SettingsContentModel>(_indexPath);
    }

    public void Refresh()
    {
    }
}
