using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArenaChipView : AbstractSkillView
{
    public override ISkillModel GetSkillModel()
    {
        return RunManager.Get<ISkillModel>(GetIndexPath());
    }

    public override void OnDrop(PointerEventData eventData) { }
}
