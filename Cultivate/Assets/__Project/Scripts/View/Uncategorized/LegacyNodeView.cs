
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LegacyNodeView : SimpleView
{
    public BreathingButton _breathingButton;
    public TMP_Text NameText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _breathingButton.ClickNeuron.Join(OnPointerClick);
    }

    public override void Refresh()
    {
        base.Refresh();
        RunNode runNode = Get<RunNode>();

        gameObject.SetActive(runNode != null);
        if (runNode == null)
            return;

        if (NameText != null)
            NameText.text = runNode.GetName() + runNode.State.ToString();

        _breathingButton.SetSprite(runNode.GetSprite());

        switch (runNode.State)
        {
            case RunNode.RunNodeState.Missed:
            case RunNode.RunNodeState.Passed:
            case RunNode.RunNodeState.Current:
                _breathingButton.SetColor(Color.black);
                break;
            case RunNode.RunNodeState.ToChoose:
            case RunNode.RunNodeState.Untouched:
                _breathingButton.SetColor(Color.white);
                break;
        }

        _breathingButton.SetBreathing(runNode.State == RunNode.RunNodeState.ToChoose);
    }

    private void OnPointerClick(PointerEventData eventData)
    {
        TryClickNode();
        // CanvasManager.Instance.RunCanvas.Refresh();
    }

    private bool TryClickNode()
    {
        RunNode runNode = Get<RunNode>();
        if (runNode.State != RunNode.RunNodeState.ToChoose || !RunManager.Instance.Environment.Map.Choosing)
            return false;

        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.MakeChoiceProcedure(runNode);
        PanelS panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
        return true;
    }
}
