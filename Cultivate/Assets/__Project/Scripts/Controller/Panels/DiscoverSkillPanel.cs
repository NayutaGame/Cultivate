using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DiscoverSkillPanel : Panel
{
    public TMP_Text DetailedText;
    public RunSkillView[] SkillViews;

    private InteractDelegate InteractDelegate;

    public override void Configure()
    {
        base.Configure();

        ConfigureInteractDelegate();
        SkillViews.Do(v => v.SetDelegate(InteractDelegate));
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new InteractDelegate(1,
            getId: view => 0,
            lMouseTable: new Func<IInteractable, bool>[]
            {
                TrySelectOption,
            }
        );
    }

    public override void Refresh()
    {
        base.Refresh();

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        DiscoverSkillPanelDescriptor d = runNode.CurrentPanel as DiscoverSkillPanelDescriptor;

        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < SkillViews.Length; i++)
        {
            bool active = i < d.GetSkillCount() && !RunManager.Instance.Map.Selecting;
            SkillViews[i].gameObject.SetActive(active);
            if(!active)
                continue;

            SkillViews[i].Configure(new IndexPath($"Run.CurrentNode.CurrentPanel.Skills#{i}"));
            SkillViews[i].Refresh();
        }
    }

    public bool TrySelectOption(IInteractable view)
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        DiscoverSkillPanelDescriptor d = runNode.CurrentPanel as DiscoverSkillPanelDescriptor;

        RunSkill skill = DataManager.Get<RunSkill>(view.GetIndexPath());
        PanelDescriptor panelDescriptor = RunManager.Instance.Map.ReceiveSignal(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        RunCanvas.Instance.SetIndexPathForPreview(null);
        return true;
    }
}
