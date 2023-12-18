
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigPanel : CurtainPanel
{
    [Header("Character Picker")]
    [SerializeField] private ListView CharacterListView;
    private int? _selectionIndex;
    private CharacterProfileView _selection;

    [Header("Difficulty Picker")]
    [SerializeField] private DifficultyPickerView DifficultyPickerView;

    [Header("Progress")]
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button StartRunButton;

    [Header("Detailed")]
    [SerializeField] private DetailedCharacterProfileView DetailedCharacterProfileView;

    public override void Configure()
    {
        base.Configure();

        DifficultyPickerView.Configure();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);

        StartRunButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.AddListener(StartRun);

        DefineInteractHandler();
        CharacterListView.SetAddress(new Address("Profile.ProfileList.Current.CharacterProfileList"));
        SelectCharacterProfile(0);
    }

    public override void Refresh()
    {
        base.Refresh();
        DifficultyPickerView.Refresh();
        CharacterListView.Refresh();
        DetailedCharacterProfileView.Refresh();
    }

    #region IInteractable

    private InteractHandler _interactHandler;
    public InteractHandler GetDelegate() => _interactHandler;
    private void DefineInteractHandler()
    {
        _interactHandler = new(1,
            getId: d =>
            {
                if (d is CharacterProfileInteractBehaviour)
                    return 0;
                return null;
            });

        _interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, SelectCharacterProfile);

        CharacterListView.SetHandler(_interactHandler);
    }

    #endregion

    private void Return()
    {
        CanvasManager.Instance.AppCanvas.CloseRunConfigPanel();
    }

    private void StartRun()
    {
        // generate form from player chose condition, to avoid save ui variables inside model
        // this form should contains: selected character, selected difficulty, selected mutators, selected seed

        // generate form
        // new RunConfigForm();

        AppManager.Push(new RunAppS());
    }

    private void SelectCharacterProfile(int i)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = CharacterListView.BehaviourFromIndex(i).GetComponent<CharacterProfileView>();
        _selectionIndex = i;

        if (_selection != null)
        {
            DetailedCharacterProfileView.SetAddress(_selection.GetAddress());
            DetailedCharacterProfileView.Refresh();
            _selection.SetSelected(true);
        }
    }

    private void SelectCharacterProfile(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = interactBehaviour.AddressBehaviour.GetComponent<CharacterProfileView>();
        _selectionIndex = CharacterListView.IndexFromBehaviour(_selection);

        if (_selection != null)
        {
            DetailedCharacterProfileView.SetAddress(_selection.GetAddress());
            DetailedCharacterProfileView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
