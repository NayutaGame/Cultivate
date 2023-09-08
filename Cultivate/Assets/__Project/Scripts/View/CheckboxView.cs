using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxView : MonoBehaviour, IIndexPath
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;
    [SerializeField] private Image ButtonImage;

    [SerializeField] private Sprite OnImage;
    [SerializeField] private Sprite OffImage;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private CheckboxModel _model;

    private void Awake()
    {
        if (_model == null)
            Configure(null);
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _model = _indexPath == null ? CheckboxModel.Default : DataManager.Get<CheckboxModel>(_indexPath);

        if (LabelText != null)
            LabelText.text = _model.Name;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Toggle);
    }

    public void Refresh()
    {
    }

    public void Toggle()
    {
        _model.Toggle();
        ButtonImage.sprite = _model.Value ? OnImage : OffImage;
    }
}
