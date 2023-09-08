using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAppS : AppS
{
    public override void Enter()
    {
        base.Enter();
        CanvasManager.Instance.AppCanvas.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.AppCanvas.gameObject.SetActive(false);
    }
}
