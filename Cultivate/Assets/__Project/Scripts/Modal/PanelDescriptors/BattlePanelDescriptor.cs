using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePanelDescriptor : PanelDescriptor
{
    private EntityEntry _entityEntry;
    public EntityEntry EntityEntry => _entityEntry;

    private CreateEntityDetails _createEntityDetails;

    public BattlePanelDescriptor(EntityEntry entityEntry, CreateEntityDetails createEntityDetails)
    {
        _entityEntry = entityEntry;
        _createEntityDetails = createEntityDetails;
    }

    public override void DefaultEnter()
    {
        RunManager.Instance.Battle.Enemy = new RunEntity(_entityEntry, _createEntityDetails);
    }
}
