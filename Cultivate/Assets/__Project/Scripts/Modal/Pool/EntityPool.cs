using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class EntityPool : Pool<RunEntity>
{
    public EntityPool()
    {
        Populate(Encyclopedia.EntityEditableList.Traversal());
    }

    public bool TryDrawEntity(out RunEntity template, DrawEntityDetails d)
    {
        Shuffle();
        bool success = TryPopItem(out template);
        if (success)
            return true;

        template = RunEntity.Default;
        return false;
    }
}
