
using TMPro;
using UnityEngine;

public class PackView : XView
{
    [SerializeField] private TMP_Text NameText;

    private HighlightBehaviour _highlightBehaviour;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _highlightBehaviour = GetBehaviour<HighlightBehaviour>();
    }
    
    public void SetHighlight(bool highlight)
    {
        if (_highlightBehaviour != null)
            _highlightBehaviour.SetHighlight(highlight);
    }

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
