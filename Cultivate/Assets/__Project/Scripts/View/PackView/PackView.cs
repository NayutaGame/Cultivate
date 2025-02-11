
using TMPro;
using UnityEngine;

public class PackView : XView
{
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        IPack pack = Get<IPack>();
        SetName(pack.GetName());
    }

    protected virtual void SetName(string name)
    {
        NameText.text = name;
    }
}
