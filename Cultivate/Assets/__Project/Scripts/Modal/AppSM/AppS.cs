using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class AppS
{
    public virtual async Task Enter(NavigateDetails d) { }
    public virtual async Task Exit(NavigateDetails d) { }
    public virtual async Task CEnter(NavigateDetails d) { }
    public virtual async Task CExit(NavigateDetails d) { }
}
