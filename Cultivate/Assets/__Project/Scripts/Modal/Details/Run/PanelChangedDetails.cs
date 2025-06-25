
public class PanelChangedDetails : RunClosureDetails
{
    public Cell FromPanel;
    public Cell ToPanel;
    
    public PanelChangedDetails(Cell fromPanel, Cell toPanel)
    {
        FromPanel = fromPanel;
        ToPanel = toPanel;
    }
}
