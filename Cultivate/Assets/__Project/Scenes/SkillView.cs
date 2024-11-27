
using TMPro;
using UnityEngine;

public class SkillView : XView
{
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        SkillModel skill = Get<SkillModel>();

        NameText.text = skill.Name;
    }
}
