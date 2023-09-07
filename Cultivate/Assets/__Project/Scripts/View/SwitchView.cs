
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchView : MonoBehaviour, IIndexPath
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button NextButton;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private SwitchModel _model;

    private void Awake()
    {
        if (_model == null)
            Configure(null);
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _model = _indexPath == null ? SwitchModel.Default : DataManager.Get<SwitchModel>(_indexPath);

        if (LabelText != null)
            LabelText.text = _model.Label;

        PrevButton.onClick.RemoveAllListeners();
        PrevButton.onClick.AddListener(Prev);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(Next);
    }

    public void Refresh()
    {
    }

    private void Prev()
    {
        _model.Prev();
        ContentText.text = _model.GetContentText();
    }

    private void Next()
    {
        _model.Next();
        ContentText.text = _model.GetContentText();;
    }
}
