
public class PanelChangedDetails : ClosureDetails
{
    public PanelDescriptor FromPanel;
    public PanelDescriptor ToPanel;
    
    public PanelChangedDetails(PanelDescriptor fromPanel, PanelDescriptor toPanel)
    {
        FromPanel = fromPanel;
        ToPanel = toPanel;
    }
}
