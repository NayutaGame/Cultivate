
using UnityEngine;
using UnityEngine.UI;

public class CharacterRunConfigTabView : RunConfigTabView
{
    [SerializeField] private Image Image;

    public override void Refresh()
    {
        base.Refresh();
        CharacterRunConfigTabControl control = Get<CharacterRunConfigTabControl>();
        CharacterEntry characterEntry = control.GetSelectedCharacterProfile().GetEntry();
        Image.sprite = characterEntry.GetRunConfigTab();
        RowLabel.text = $"角色：{characterEntry.GetName()}";
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AppManager.Instance.ConfigManager.CharacterTabControl.CharacterSelectNeuron.Join(CharacterChanged);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        AppManager.Instance.ConfigManager.CharacterTabControl.CharacterSelectNeuron.Remove(CharacterChanged);
    }

    private void CharacterChanged(CharacterSelectDetails d)
    {
        CharacterEntry characterEntry = d.ToCharacter.GetEntry();
        Image.sprite = characterEntry.GetRunConfigTab();
        RowLabel.text = $"角色：{characterEntry.GetName()}";
    }
}