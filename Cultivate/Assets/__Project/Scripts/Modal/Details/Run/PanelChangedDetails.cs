
public class PanelChangedDetails : RunClosureDetails
{
    public ICellAdapter FromPanel;
    public ICellAdapter ToPanel;
    
    public PanelChangedDetails(ICellAdapter fromPanel, ICellAdapter toPanel)
    {
        FromPanel = fromPanel;
        ToPanel = toPanel;
    }
}
