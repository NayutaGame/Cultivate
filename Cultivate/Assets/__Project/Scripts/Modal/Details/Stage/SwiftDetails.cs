
public class SwiftDetails : EventDetails
{
    public StageEntity Owner;
    public bool Swift;
    public bool TriSwift;
    public bool OctSwift;

    public SwiftDetails(StageEntity owner, bool swift, bool triSwift, bool octSwift)
    {
        Owner = owner;
        Swift = swift;
        TriSwift = triSwift;
        OctSwift = octSwift;
    }
}
