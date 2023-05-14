using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscoverSkillPanel : Panel
{
    public TMP_Text DetailedText;
    public RunSkillView[] SkillViews;

    public override void Refresh()
    {
        base.Refresh();

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        DiscoverSkillPanelDescriptor d = runNode.CurrentPanel as DiscoverSkillPanelDescriptor;

        if (d.GetDetailedText() != null)
            DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < SkillViews.Length; i++)
        {
            bool active = i < d.GetSkillCount() && !RunManager.Instance.Map.Selecting;
            SkillViews[i].gameObject.SetActive(active);
            if(!active)
                continue;

            SkillViews[i].Configure(new IndexPath($"CurrentNode.CurrentPanel.Skills#{i}"));
            SkillViews[i].Refresh();
        }
    }
}
