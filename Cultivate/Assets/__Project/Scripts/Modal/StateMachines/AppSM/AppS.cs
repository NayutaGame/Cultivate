
using Cysharp.Threading.Tasks;

public class AppS
{
    public virtual async UniTask<Config> CEnter(NavigateDetails d) => new();
    public virtual async UniTask Enter(NavigateDetails d, Config config) { }
    public virtual async UniTask<Result> Exit(NavigateDetails d) => new();
    public virtual async UniTask CExit(NavigateDetails d, Result result) { }
}
