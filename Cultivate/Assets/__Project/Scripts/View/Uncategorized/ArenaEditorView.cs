
using CLLibrary;
using UnityEngine.UI;

public class ArenaEditorView : ListView
{
    public Button RandomButton;
    public Button CompeteButton;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        RandomButton.onClick.AddListener(Random);
        CompeteButton.onClick.AddListener(Compete);
    }

    private void Random()
    {
        // ActivePool.Do(v => ((MutableEntityView)v).RandomButton.onClick.Invoke());
        // multiple refreshes
    }

    private void Compete()
    {
        Arena arena = Get<Arena>();
        arena.Compete();
        RunCanvas.Instance.Refresh();
    }
}
