
public class InvalidMergeTarget : MergeTarget
{
    public InvalidMergeTarget(string mergeType, string errorMessage) : base(mergeType, false, errorMessage, null, null, null, null)
    {
    }
}