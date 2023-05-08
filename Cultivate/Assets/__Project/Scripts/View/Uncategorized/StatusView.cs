using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    public TMP_Dropdown JingJieDropdown;

    public void Configure()
    {
        JingJieDropdown.options = new List<TMP_Dropdown.OptionData>();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);
    }

    public void Refresh()
    {
    }

    public void JingJieChanged(int index)
    {
        RunManager.Instance.Map.JingJie = index;
        RunCanvas.Instance.Refresh();
    }
}
