
public interface IMarkedSliderModel
{
    int GetMin();
    int GetMax();
    int? GetValue();
    Address GetMarkListModelAddress(Address address);
}
