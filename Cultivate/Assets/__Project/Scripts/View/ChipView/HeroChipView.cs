using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroChipView : RunChipView
{
    private bool _reveal;

    public override void Refresh()
    {
        base.Refresh();
    }

    public override void OnDrop(PointerEventData eventData)
    {
    }
}
