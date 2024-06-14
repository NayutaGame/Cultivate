
using CLLibrary;

public class PanelSM : StateMachine<PanelS>
{
    private Panel[] _list;
    
    public PanelSM(Panel[] list)
    {
        _list = list;
    }

    public Panel this[PanelS panelS] => _list[panelS.Index];

    public Panel GetCurrPanel()
    {
        return _list[State.Index];
    }
}
