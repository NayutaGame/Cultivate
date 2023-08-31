using System;
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

    public Func<PanelDescriptor> _win;
    public Func<PanelDescriptor> _lose;

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is BattleResultSignal battleResultSignal)
        {
            return (battleResultSignal.State == BattleResultSignal.BattleResultState.Win ? _win : _lose).Invoke();
        }

        return this;
    }
}
