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

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");

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

        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < SkillViews.Length; i++)
        {
            bool active = i < d.GetSkillCount() && !RunManager.Instance.Battle.Map.Selecting;
            SkillViews[i].gameObject.SetActive(active);
            if(!active)
                continue;

            SkillViews[i].SetAddress(_address.Append($".Skills#{i}"));
            SkillViews[i].Refresh();
        }
    }

    public bool TrySelectOption(IInteractable view)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        RunSkill skill = view.Get<RunSkill>();
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        return true;
    }
}
