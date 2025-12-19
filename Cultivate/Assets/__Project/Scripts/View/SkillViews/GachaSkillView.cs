
using UnityEngine.EventSystems;

public class GachaSkillView : SkillView
{
    private void OnDisable()
    {
        GetBehaviour<ContentBehaviour>().Slot.GetInteractBehaviour().LeftClickNeuron.Remove(Gacha);
    }

    private void Gacha(InteractBehaviour ib, PointerEventData d)
    {
        Get<GachaItem>().GachaProcedure();
    }

    public void SetPicking(bool picking)
    {
        InteractBehaviour ib = GetBehaviour<ContentBehaviour>().Slot.GetInteractBehaviour();
        if (picking)
        {
            ib.LeftClickNeuron.Join(Gacha);
            GetBehaviour<AnnotationBehaviour>().SetInteractBehaviour(null);
        }
        else
        {
            ib.LeftClickNeuron.Remove(Gacha);
            GetBehaviour<AnnotationBehaviour>().SetInteractBehaviour(ib);
        }
    }
}
