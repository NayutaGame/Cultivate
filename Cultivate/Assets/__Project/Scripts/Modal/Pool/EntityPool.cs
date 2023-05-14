using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class EntityPool : Pool<EntityEntry>
{
    public EntityPool()
    {
        Populate(Encyclopedia.EntityCategory.Traversal.FilterObj(entityEntry => entityEntry != Encyclopedia.EntityCategory[0]).ToList());
    }

    public bool ForceDrawEntityEntry(out EntityEntry entityEntry, CreateEntityDetails d)
    {
        Shuffle();
        TryPopItem(out EntityEntry item, e => e.CanCreate(d));
        entityEntry = item ?? Encyclopedia.EntityCategory[0];
        return true;
    }
}
