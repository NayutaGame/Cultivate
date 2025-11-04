
using System;
using UnityEngine;

[Serializable]
public class SetCharacterNameCommend : Commend
{
    [SerializeField] private string CharacterName;
    [SerializeField] private bool IsHome;
    
    public override void Execute()
    {
        RunManager.Instance.Environment.SetCharacterNameProcedure(CharacterName, IsHome);
    }
}