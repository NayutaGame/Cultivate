
using UnityEngine.EventSystems;

public class GachaSkillView : SkillView
{
    private void OnDisable()
    {
        GetBehaviour<ContentBehaviour>().SlotView.GetInteractBehaviour().LeftClickNeuron.Remove(Gacha);
    }

    private void Gacha(InteractBehaviour ib, PointerEventData d)
    {
        Get<GachaItem>().GachaProcedure();
    }

    public void SetPicking(bool picking)
    {
        if (picking)
            GetBehaviour<ContentBehaviour>().SlotView.GetInteractBehaviour().LeftClickNeuron.Join(Gacha);
        else
            GetBehaviour<ContentBehaviour>().SlotView.GetInteractBehaviour().LeftClickNeuron.Remove(Gacha);
    }
}
