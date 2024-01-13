
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffView : SimpleView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text StackText;

    public override void Refresh()
    {
        base.Refresh();

        Buff b = Get<Buff>();

        NameText.text = $"{b.GetName()}";
        StackText.text = $"{b.Stack}";
    }
}
