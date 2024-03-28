
public class DrawEntityDetails
{
    public JingJie JingJie;
    public bool AllowNormal;
    public bool AllowElite;
    public bool AllowBoss;

    public DrawEntityDetails(JingJie jingJie, bool allowNormal = false, bool allowElite = false, bool allowBoss = false)
    {
        JingJie = jingJie;
        AllowNormal = allowNormal;
        AllowElite = allowElite;
        AllowBoss = allowBoss;
    }

    public bool CanDraw(RunEntity entity)
    {
        bool jingJieIsMatch = entity.GetJingJie() == JingJie;
        bool powerIsMatch = (entity.IsNormal() && AllowNormal)
                            || (entity.IsElite() && AllowElite)
                            || (entity.IsBoss() && AllowBoss);
        return jingJieIsMatch && powerIsMatch;
    }
}
