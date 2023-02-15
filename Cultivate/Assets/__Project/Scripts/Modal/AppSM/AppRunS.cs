using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppRunS : AppS
{
    public override void Enter()
    {
        base.Enter();
        CanvasManager.Instance.RunCanvas.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.RunCanvas.SetActive(false);
    }

    public override void CEnter()
    {
        base.CEnter();
        CanvasManager.Instance.RunCanvas.SetActive(false);
    }

    public override void CExit()
    {
        base.CExit();
        CanvasManager.Instance.RunCanvas.SetActive(true);
    }
}
