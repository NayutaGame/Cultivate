using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ArenaEditorView : InventoryView<ArenaEnemyView>
{
    public Button RandomButton;
    public Button CompeteButton;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        RandomButton.onClick.AddListener(Random);
        CompeteButton.onClick.AddListener(Compete);
    }

    private void Random()
    {
        Views.Do(v => v.RandomEnemyButton.onClick.Invoke());
        // multiple refreshes
    }

    private void Compete()
    {
        Arena arena = RunManager.Get<Arena>(GetIndexPath());
        arena.Compete();
        RunCanvas.Instance.Refresh();
    }
}
