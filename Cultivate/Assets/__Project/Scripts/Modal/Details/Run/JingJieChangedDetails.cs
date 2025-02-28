
public class JingJieChangedDetails : RunClosureDetails
{
    public JingJie FromJingJie;
    public JingJie ToJingJie;

    public JingJieChangedDetails(JingJie fromJingJie, JingJie toJingJie)
    {
        FromJingJie = fromJingJie;
        ToJingJie = toJingJie;
    }
}
