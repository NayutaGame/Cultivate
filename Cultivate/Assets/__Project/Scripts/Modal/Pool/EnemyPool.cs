using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class EnemyPool : AutoPool<EnemyEntry>
{
    public EnemyPool() : base(Encyclopedia.EnemyCategory.Traversal.ToList())
    {
    }
}
