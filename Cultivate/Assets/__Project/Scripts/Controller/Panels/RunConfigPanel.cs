
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigPanel : Panel
{
    [SerializeField] private GameObject DemoLockedSign;
    [SerializeField] private ListView CharacterListView;
    [SerializeField] private DetailedCharacterProfileView DetailedCharacterProfileView;
    
    [SerializeField] private DifficultyPickerView DifficultyPickerView;
    [SerializeField] private CLButtonPatternA ReturnButton;
    [SerializeField] private CLButtonPatternA StartRunButton;

    [SerializeField] private CLButtonPatternA PackConfigButton;

    [SerializeField] private PackConfigPanel PackConfigPanel;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        PackConfigPanel.CheckAwake();

        DifficultyPickerView.Configure();

        CharacterListView.SetAddress(new Address("Profile.ProfileList.Current.CharacterProfileList"));
        CharacterListView.LeftClickNeuron.Join(Select);
    }

    public override void Refresh()
    {
        DemoLockedSign.SetActive(AppManager.Instance.PackageIsDemo() || AppManager.Instance.PackageIsForStream());
        CharacterListView.Refresh();
        AppManager.Instance.ConfigManager.SelectFirstCharacter();
        RefreshAllSelection();
        DifficultyPickerView.Refresh();
        DetailedCharacterProfileView.Refresh();
    }

    private void OnEnable()
    {
        ReturnButton.LeftClickNeuron.Add(Return);
        ReturnButton.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
        ReturnButton.GetInteractBehaviour().PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        StartRunButton.LeftClickNeuron.Add(StartRun);
        StartRunButton.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
        StartRunButton.GetInteractBehaviour().PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        PackConfigButton.LeftClickNeuron.Add(EnterPackConfig);
        PackConfigButton.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
        PackConfigButton.GetInteractBehaviour().PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        
        CharacterListView.Sync();
        Refresh();
        AppManager.Instance.PushEscFunc(Return);
    }

    private void OnDisable()
    {
        ReturnButton.LeftClickNeuron.Remove(Return);
        ReturnButton.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        ReturnButton.GetInteractBehaviour().PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        StartRunButton.LeftClickNeuron.Remove(StartRun);
        StartRunButton.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        StartRunButton.GetInteractBehaviour().PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        PackConfigButton.LeftClickNeuron.Remove(EnterPackConfig);
        PackConfigButton.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        PackConfigButton.GetInteractBehaviour().PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        
        AppManager.Instance.PopEscFunc();
    }

    private void Return(InteractBehaviour ib, PointerEventData d)
        => Return();

    private void Return()
    {
        CloseRunConfigPanel();
    }

    private void EnterPackConfig(InteractBehaviour ib, PointerEventData d)
    {
        PackConfigPanel.SetUnmodifiedPackPreset(AppManager.Instance.ConfigManager.WriteCurrentIntoPackPreset());
        PackConfigPanel.GetAnimator().SetStateAsync(1);
    }

    private async UniTask CloseRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(0);
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(1);
    }

    private void StartRun(InteractBehaviour ib, PointerEventData d)
    {
        CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;
        List<PackEntry> packEntries = AppManager.Instance.ConfigManager.GetEquippedPacks();
        RunConfig runConfig = new(characterProfile, DifficultyPickerView.GetSelection(), packEntries);
        AppManager.Instance.Push(AppStateMachine.RUN, runConfig);
    }
    
    private void Select(InteractBehaviour ib, PointerEventData eventData)
        => Select((ib.GetView() as SlotView).GetContentView().GetBehaviour<SelectBehaviour>());
    
    private void Select(SelectBehaviour selectBehaviour)
    {
        SelectBehaviour currentCharacterSelectBehaviour = GetCurrentCharacterSelectBehaviour();
        if (currentCharacterSelectBehaviour != null)
            currentCharacterSelectBehaviour.SetSelectAsync(false);

        currentCharacterSelectBehaviour = selectBehaviour;
        AppManager.Instance.ConfigManager.SelectCharacterProcedure(new CharacterSelectDetails(currentCharacterSelectBehaviour.Get<CharacterProfile>()));
    
        if (currentCharacterSelectBehaviour != null)
        {
            DetailedCharacterProfileView.SetAddress(currentCharacterSelectBehaviour.GetAddress());
            DetailedCharacterProfileView.Refresh();
            currentCharacterSelectBehaviour.SetSelectAsync(true);
        }

        RefreshStartRunButton();
    }

    private SelectBehaviour GetCurrentCharacterSelectBehaviour()
    {
        // 从model中获取当前选中的character
        CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;
        int? index = CharacterListView.Traversal().FirstIdx(v => v.Get<CharacterProfile>() == characterProfile);
        if (index == null)
            return null;
        return CharacterListView.ViewFromIndex(index.Value).GetContentView().GetBehaviour<SelectBehaviour>();
    }

    private void RefreshAllSelection()
    {
        SelectBehaviour currentCharacterSelectBehaviour = GetCurrentCharacterSelectBehaviour();
        foreach (var slotView in CharacterListView.Traversal())
        {
            SelectBehaviour s = slotView.GetContentView().GetBehaviour<SelectBehaviour>();
            s.SetSelect(s == currentCharacterSelectBehaviour);
        }

        DetailedCharacterProfileView.SetAddress(currentCharacterSelectBehaviour.GetAddress());
        DetailedCharacterProfileView.Refresh();

        RefreshStartRunButton();
    }

    public void RefreshStartRunButton()
    {
        CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;

        bool interactable = characterProfile.IsUnlocked() &&
                            !characterProfile.IsDemoLocked() &&
                            !characterProfile.IsForStreamLocked() &&
                            !DifficultyPickerView.GetSelection().IsDemoLocked() &&
                            DifficultyPickerView.GetSelection().IsUnlocked();
        
        StartRunButton.SetStateToActiveIf(interactable);
    }
}
