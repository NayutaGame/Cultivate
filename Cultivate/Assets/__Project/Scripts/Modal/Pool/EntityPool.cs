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

    public bool TryDrawEntity(out EntityEntry entityEntry, DrawEntityDetails d)
    {
        Shuffle();
        TryPopItem(out EntityEntry item);
        entityEntry = item ?? Encyclopedia.EntityCategory[0];
        return true;
    }
}
