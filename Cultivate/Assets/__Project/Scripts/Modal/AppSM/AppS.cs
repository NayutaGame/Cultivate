
using System.Threading.Tasks;

public class AppS
{
    public virtual async Task<Config> CEnter(NavigateDetails d) => new();
    public virtual async Task Enter(NavigateDetails d, Config config) { }
    public virtual async Task<Result> Exit(NavigateDetails d) => new();
    public virtual async Task CExit(NavigateDetails d, Result result) { }
}
