
using System;
using System.Threading.Tasks;
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    [NonSerialized] public RectTransform RectTransform;

    public virtual void Configure()
    {
        RectTransform ??= GetComponent<RectTransform>();

        _sm = new(2);
        // 0 for hide, 1 for show
        _sm.SetElement(0, 1, new CommandFromTween(GetHandle, SetHandle, ShowAnimation().SetAutoKill()));
        _sm.SetElement(1, 0, new CommandFromTween(GetHandle, SetHandle, HideAnimation().SetAutoKill()));
        
    }

    public virtual void Refresh() { }

    private CommandStateMachine _sm;
    private Tween _handle;
    private Tween GetHandle() => _handle;
    private void SetHandle(Tween value) => _handle = value;

    public void SetState(int state)
    {
        _sm.SetState(state);
    }

    public async Task AsyncSetState(int state)
    {
        await _sm.AsyncSetState(state);
    }

    public async Task ToggleShowing()
        => await AsyncSetState(_sm.GetState() != 0 ? 0 : 1);

    public virtual Tween ShowAnimation()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(true));

    public virtual Tween HideAnimation()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));
}
