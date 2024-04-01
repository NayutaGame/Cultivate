
public class SetJingJieDetails : EventDetails
{
    public JingJie FromJingJie;
    public JingJie ToJingJie;

    public SetJingJieDetails(JingJie fromJingJie, JingJie toJingJie)
    {
        FromJingJie = fromJingJie;
        ToJingJie = toJingJie;
    }
}
