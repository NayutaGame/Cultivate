
using System;
using UnityEngine;

[Serializable]
public class SetCharacterCommend : Commend
{
    [SerializeField] private string CharacterName;
    [SerializeField] private bool IsHome;
    
    public override void Execute()
    {
        RunManager.Instance.Environment.SetCharacterProcedure(CharacterName, IsHome);
    }
}