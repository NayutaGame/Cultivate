
public class RunClosureDetails
{
    public RunClosureListener Listener;
    public bool Cancel;
    public bool Induced;

    protected RunClosureDetails()
    {
        Cancel = false;
        Induced = false;
    }
}
