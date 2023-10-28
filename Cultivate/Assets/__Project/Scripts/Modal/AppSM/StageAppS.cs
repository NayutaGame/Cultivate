using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(true);
        StageManager.Instance.Enter();
    }

    public override async Task Exit()
    {
        await base.Exit();
        StageManager.Instance.Exit();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(false);
        AppManager.Instance.StageManager.gameObject.SetActive(false);
    }
}
