using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanelDescriptor : PanelDescriptor
{
    public string GetDetailedText() => "Detailed Text";

    public int GetOptionsCount() => 4;
    public string GetOption(int i) => $"Option {i}";

    public bool CanSelectOption(int i) => true;

    public void SelectOption(int i)
    {
        // 反馈到用户定义的行为里面

        // 经常是将指针移至下一个PanelDescriptor

        if (i == 0)
        {
            // 移动指针
        }
        else if (i == 1)
        {
            // 移动指针
        }
    }
}
