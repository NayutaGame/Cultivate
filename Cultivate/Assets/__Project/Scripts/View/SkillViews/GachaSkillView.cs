
using UnityEngine.EventSystems;

public class GachaSkillView : SkillView
{
    private void OnDisable()
    {
        GetBehaviour<ContentBehaviour>().Slot.GetInteractBehaviour().NeuronBundle.LeftClickNeuron.Remove(Gacha);
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
            ib.NeuronBundle.LeftClickNeuron.Join(Gacha);
            GetBehaviour<AnnotationBehaviour>().SetInteractBehaviour(null);
        }
        else
        {
            ib.NeuronBundle.LeftClickNeuron.Remove(Gacha);
            GetBehaviour<AnnotationBehaviour>().SetInteractBehaviour(ib);
        }
    }
}
