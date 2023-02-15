using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStageS : AppS
{
    public override void Enter()
    {
        base.Enter();
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.Refresh();
        StageManager.Instance.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        StageManager.Instance.Exit();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(false);
        AppManager.Instance.StageManager.gameObject.SetActive(false);
    }
}
