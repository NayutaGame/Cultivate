using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ArenaEditorView : ListView<MutableEntityView>
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
        Views.Do(v => v.RandomButton.onClick.Invoke());
        // multiple refreshes
    }

    private void Compete()
    {
        Arena arena = DataManager.Get<Arena>(GetIndexPath());
        arena.Compete();
        RunCanvas.Instance.Refresh();
    }
}
