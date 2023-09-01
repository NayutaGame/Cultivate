using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMechRewardDescriptor : RewardDescriptor
{
    private AddMechDetails _addMechDetails;

    public AddMechRewardDescriptor(AddMechDetails addMechDetails = null)
    {
        _addMechDetails = addMechDetails ?? new();
    }

    public override void Claim()
    {
        RunManager.Instance.Battle.ForceAddMech(_addMechDetails);
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
