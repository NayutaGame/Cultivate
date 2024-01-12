
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigPanel : CurtainPanel
{
    [Header("Character Picker")]
    [SerializeField] private ListView CharacterListView;
    // TODO: move select into ListView
    private SelectBehaviour _selection;
    [SerializeField] private DetailedCharacterProfileView DetailedCharacterProfileView;

    [Header("Difficulty Picker")]
    [SerializeField] private DifficultyPickerView DifficultyPickerView;

    [Header("Progress")]
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button StartRunButton;

    public override void Configure()
    {
        base.Configure();

        DifficultyPickerView.Configure();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);

        StartRunButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.AddListener(StartRun);

        CharacterListView.SetAddress(new Address("Profile.ProfileList.Current.CharacterProfileList"));
        CharacterListView.LeftClickNeuron.Join(Select);

        Select(0);
    }

    public override void Refresh()
    {
        base.Refresh();
        DifficultyPickerView.Refresh();
        CharacterListView.Refresh();
        DetailedCharacterProfileView.Refresh();
    }

    private void Return()
    {
        CanvasManager.Instance.AppCanvas.CloseRunConfigPanel();
    }

    private void StartRun()
    {
        // this form should contains: selected character, selected difficulty, selected mutators, selected seed

        AppManager.Instance.ProfileManager.RunConfigForm = new RunConfigForm(
            _selection.GetSimpleView().Get<CharacterProfile>(),
            DifficultyPickerView.GetSelection());

        AppManager.Push(new RunAppS());
    }

    private void Select(int i)
        => Select(CharacterListView.ItemBehaviourFromIndex(i).GetSelectBehaviour());

    private void Select(InteractBehaviour ib, PointerEventData eventData)
        => Select(ib.GetSimpleView().GetSelectBehaviour());

    private void Select(SelectBehaviour selectBehaviour)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = selectBehaviour;

        if (_selection != null)
        {
            DetailedCharacterProfileView.SetAddress(_selection.GetSimpleView().GetAddress());
            DetailedCharacterProfileView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
