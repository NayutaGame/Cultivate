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

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("Run.Battle.Map.CurrentNode.CurrentPanel");

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

        DiscoverSkillPanelDescriptor d = DataManager.Get<DiscoverSkillPanelDescriptor>(_indexPath);

        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < SkillViews.Length; i++)
        {
            bool active = i < d.GetSkillCount() && !RunManager.Instance.Battle.Map.Selecting;
            SkillViews[i].gameObject.SetActive(active);
            if(!active)
                continue;

            SkillViews[i].Configure(new IndexPath($"{_indexPath}.Skills#{i}"));
            SkillViews[i].Refresh();
        }
    }

    public bool TrySelectOption(IInteractable view)
    {
        DiscoverSkillPanelDescriptor d = DataManager.Get<DiscoverSkillPanelDescriptor>(_indexPath);

        RunSkill skill = DataManager.Get<RunSkill>(view.GetIndexPath());
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        return true;
    }
}
