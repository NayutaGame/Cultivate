using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStageS : AppS
{
    public override void Enter()
    {
        base.Enter();
        CanvasManager.Instance.StageCanvas.SetActive(true);
        StageManager.Instance.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.StageCanvas.SetActive(false);
        StageManager.Instance.Exit();
    }
}
