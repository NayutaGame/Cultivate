using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    public TMP_Text StatusText;
    public TMP_Dropdown JingJieDropdown;

    public void Configure()
    {
        JingJieDropdown.options = new List<TMP_Dropdown.OptionData>();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);
    }

    public void Refresh()
    {
        StatusText.text = RunManager.Instance.GetStatusString();
    }

    public void JingJieChanged(int index)
    {
        RunManager.Instance.JingJie = index;
        RunCanvas.Instance.Refresh();
    }
}
