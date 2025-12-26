
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Entry/Character", -10, true)]
public class CharacterEntryNode : Node
{
    [SerializeField]
    private string Name;
    
    [SerializeField]
    private OutputPort<CharacterEntry> Value = new(self => (self as CharacterEntryNode).GetValue());

    private CharacterEntry GetValue()
    {
        if (!Application.isPlaying)
            return null;

        CharacterEntry characterEntry = Encyclopedia.CharacterCategory.FromName(Name);
        if (characterEntry == null)
            Debug.LogError("百花：没有查询到Character");
        return characterEntry;
    }
}