using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class EntityPool : AutoPool<EntityEntry>
{
    public EntityPool() : base(Encyclopedia.EntityCategory.Traversal.ToList())
    {
    }

    public bool TryDrawEntityEntry(out EntityEntry entityEntry, CreateEntityDetails d)
    {
        Shuffle();
        entityEntry = ForcePopItem(e => e.CanCreate(d));
        return true;
    }
}
