using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();
        RunManager.Instance.Enter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        RunCanvas.Instance.NodeLayer.DisableCurrentPanel();
    }

    public override async Task Exit()
    {
        await base.Exit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        RunManager.Instance.Exit();
    }

    public override async Task CEnter()
    {
        await base.CEnter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override async Task CExit()
    {
        await base.CExit();
        RunManager.Instance.CExit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
    }
}
