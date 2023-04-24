using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppRunS : AppS
{
    public override void Enter()
    {
        base.Enter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override void CEnter()
    {
        base.CEnter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override void CExit()
    {
        base.CExit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        RunManager.Instance.CExit();
    }
}
