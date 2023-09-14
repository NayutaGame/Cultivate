
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeView : MonoBehaviour, IAddress
{
    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public JumpingButton _jumpingButton;
    public TMP_Text NameText;

    public virtual void Configure(Address address)
    {
        _address = address;

        _jumpingButton.RemoveAllListeners();
        _jumpingButton.AddListener(OnPointerClick);
    }

    public virtual void Refresh()
    {
        RunNode runNode = Get<RunNode>();

        gameObject.SetActive(runNode != null);
        if (runNode == null)
            return;

        if (NameText != null)
            NameText.text = runNode.GetTitle() + runNode.State.ToString();

        _jumpingButton.SetSprite(runNode.Sprite);

        switch (runNode.State)
        {
            case RunNode.RunNodeState.Missed:
            case RunNode.RunNodeState.Passed:
            case RunNode.RunNodeState.Current:
                _jumpingButton.SetColor(Color.black);
                break;
            case RunNode.RunNodeState.ToChoose:
            case RunNode.RunNodeState.Future:
                _jumpingButton.SetColor(Color.white);
                break;
        }

        _jumpingButton.SetJumping(runNode.State == RunNode.RunNodeState.ToChoose);
    }

    private void OnPointerClick(PointerEventData eventData)
    {
        TryClickNode(GetAddress());
        RunCanvas.Instance.Refresh();
    }

    private bool TryClickNode(Address address)
    {
        RunNode runNode = Get<RunNode>();
        if (runNode.State != RunNode.RunNodeState.ToChoose || !RunManager.Instance.Battle.Map.Selecting)
            return false;

        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.SelectedNode(runNode);
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        return true;
    }
}
