
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffView : SimpleView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text StackText;
    [SerializeField] private Image IconImage;

    public override void SetAddress(Address address)
    {
        if (Get<Buff>() is { } b1)
        {
            b1.PingNeuron.Remove(PingAnimation);
            b1.StackChangedNeuron.Remove(Refresh);
        }
        base.SetAddress(address);
        if (Get<Buff>() is { } b2)
        {
            b2.PingNeuron.Add(PingAnimation);
            b2.StackChangedNeuron.Add(Refresh);
        }
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        if (Get<Buff>() is { } b1)
        {
            b1.PingNeuron.Remove(PingAnimation);
            b1.StackChangedNeuron.Remove(Refresh);
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        Buff b = Get<Buff>();
        StackText.text = b.Stack.ToString();
        IconImage.sprite = b.GetEntry().GetSprite();

        NameText.text = IconImage.sprite == Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite ? b.GetName() : "";
    }

    private void PingAnimation()
    {
        GetComponent<ExtraBehaviourPivot>()?.PlayPingAnimation();
    }
}
