
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigPanel : Panel
{
    [SerializeField] private ListView CharacterListView;
    private SelectBehaviour _selection;
    [SerializeField] private DetailedCharacterProfileView DetailedCharacterProfileView;
    
    [SerializeField] private DifficultyPickerView DifficultyPickerView;
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button StartRunButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

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
        CloseRunConfigPanel();
    }

    private async UniTask CloseRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(0);
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(1);
    }

    private void StartRun()
    {
        // this form should contains: selected character, selected difficulty, selected mutators, selected seed
        AppManager.Instance.ProfileManager.RunConfigForm = new RunConfigForm(
            _selection.Get<CharacterProfile>(),
            DifficultyPickerView.GetSelection());

        AppManager.Instance.Push(AppStateMachine.RUN);
    }

    private void Select(int i)
        => Select(CharacterListView.ViewFromIndex(i).GetBehaviour<SelectBehaviour>());
    
    private void Select(InteractBehaviour ib, PointerEventData eventData)
        => Select(ib.GetView().GetBehaviour<SelectBehaviour>());
    
    private void Select(SelectBehaviour selectBehaviour)
    {
        if (_selection != null)
            _selection.SetSelectAsync(false);
    
        _selection = selectBehaviour;
    
        if (_selection != null)
        {
            DetailedCharacterProfileView.SetAddress(_selection.GetAddress());
            DetailedCharacterProfileView.Refresh();
            _selection.SetSelectAsync(true);
        }
    }
}
