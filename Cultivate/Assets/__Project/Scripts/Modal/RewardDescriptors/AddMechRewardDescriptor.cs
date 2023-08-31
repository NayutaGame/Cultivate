using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMechRewardDescriptor : RewardDescriptor
{
    private AddMechDetails _addMechDetails;

    public AddMechRewardDescriptor(AddMechDetails addMechDetails)
    {
        _addMechDetails = addMechDetails;
    }

    public override void Claim()
    {
        RunManager.Instance.ForceAddMech(_addMechDetails);
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
