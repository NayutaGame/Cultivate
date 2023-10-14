using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAppS : AppS
{
    public override void Enter()
    {
        base.Enter();
        RunManager.Instance.Enter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        RunCanvas.Instance.NodeLayer.DisableCurrentPanel();
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        RunManager.Instance.Exit();
    }

    public override void CEnter()
    {
        base.CEnter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override void CExit()
    {
        base.CExit();
        RunManager.Instance.CExit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
    }
}
