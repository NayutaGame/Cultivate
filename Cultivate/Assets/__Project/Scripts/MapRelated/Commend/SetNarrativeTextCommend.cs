
using UnityEngine;

public class SetNarrativeTextCommend : Commend
{
    [SerializeField] private string Text;
    public override void Execute()
    {
        RunManager.Instance.Environment.SetNarrativeTextProcedure(Text);
    }
}