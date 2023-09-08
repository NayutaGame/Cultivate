using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsContentView : MonoBehaviour, IIndexPath
{
    // [SerializeField] private WidgetListView Widgets;

    [SerializeField] private GameObject SliderWidget;
    [SerializeField] private GameObject CheckboxWidget;
    [SerializeField] private GameObject ButtonWidget;
    [SerializeField] private GameObject SwitchWidget;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private SettingsContentModel _model;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _model = DataManager.Get<SettingsContentModel>(_indexPath);

        // _model.Widgets
    }

    public void Refresh()
    {
    }
}
