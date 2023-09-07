using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour, IIndexPath
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private ButtonModel _model;

    private void Awake()
    {
        if (_model == null)
            Configure(null);
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _model = _indexPath == null ? ButtonModel.Default : DataManager.Get<ButtonModel>(_indexPath);

        if (LabelText != null)
            LabelText.text = _model.Label;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Click);
    }

    public void Refresh()
    {
    }

    public void Click()
    {
        _model.Click();
    }
}
