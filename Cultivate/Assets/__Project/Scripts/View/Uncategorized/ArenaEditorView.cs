using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ArenaEditorView : ListView<MutableEntityView>
{
    public Button RandomButton;
    public Button CompeteButton;

    public override void Configure(Address address)
    {
        base.Configure(address);

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
        Arena arena = Get<Arena>();
        arena.Compete();
        RunCanvas.Instance.Refresh();
    }
}
