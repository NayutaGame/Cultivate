
public class CellChangedDetails : RunClosureDetails
{
    public ICellAdapter FromCell;
    public ICellAdapter ToCell;
    
    public CellChangedDetails(ICellAdapter fromCell, ICellAdapter toCell)
    {
        FromCell = fromCell;
        ToCell = toCell;
    }
}
