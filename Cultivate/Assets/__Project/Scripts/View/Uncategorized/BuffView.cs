
using TMPro;

public class BuffView : ItemView
{
    public TMP_Text NameText;
    public TMP_Text StackText;

    public override void Refresh()
    {
        // TryGetHeroBuff
        Buff b = Get<Buff>();

        gameObject.SetActive(b != null);
        if (b == null) return;

        NameText.text = $"{b.GetName()}";
        StackText.text = $"{b.Stack}";
    }
}
