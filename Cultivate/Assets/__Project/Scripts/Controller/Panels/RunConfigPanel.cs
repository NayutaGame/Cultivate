
using System;
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigPanel : Panel
{
    [SerializeField] private ListView CharacterListView;
    [SerializeField] private DetailedCharacterProfileView DetailedCharacterProfileView;
    
    [SerializeField] private DifficultyPickerView DifficultyPickerView;
    [SerializeField] private XButton ReturnButton;
    [SerializeField] private XButton StartRunButton;

    [SerializeField] private XButton PackConfigButton;

    [SerializeField] private PackConfigPanel PackConfigPanel;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        PackConfigPanel.CheckAwake();

        DifficultyPickerView.Configure();

        ReturnButton._button.onClick.RemoveAllListeners();
        ReturnButton._button.onClick.AddListener(Return);
        ReturnButton._button.onClick.AddListener(AudioManager.PlayButtonPress);

        ReturnButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;

        StartRunButton._button.onClick.RemoveAllListeners();
        StartRunButton._button.onClick.AddListener(StartRun);
        StartRunButton._button.onClick.AddListener(AudioManager.PlayButtonPress);

        StartRunButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        
        PackConfigButton._button.onClick.RemoveAllListeners();
        PackConfigButton._button.onClick.AddListener(EnterPackConfig);
        PackConfigButton._button.onClick.AddListener(AudioManager.PlayButtonPress);

        PackConfigButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;

        CharacterListView.SetAddress(new Address("Profile.ProfileList.Current.CharacterProfileList"));
        CharacterListView.LeftClickNeuron.Join(Select);
    }

    public override void Refresh()
    {
        DifficultyPickerView.Refresh();
        CharacterListView.Refresh();
        RefreshAllSelection();
        DetailedCharacterProfileView.Refresh();
    }

    private void OnEnable()
    {
        Refresh();
        AppManager.Instance.PushEscFunc(Return);

    }

    private void OnDisable()
    {
        AppManager.Instance.PopEscFunc();
    }

    private void Return()
    {
        CloseRunConfigPanel();
    }

    private void EnterPackConfig()
    {
        PackConfigPanel.SetUnmodifiedPackPreset(AppManager.Instance.ConfigManager.WriteCurrentIntoPackPreset());
        PackConfigPanel.GetAnimator().SetStateAsync(1);
    }

    private async UniTask CloseRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(0);
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(1);
    }

    private void StartRun()
    {
        CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;
        List<PackEntry> packEntries = AppManager.Instance.ConfigManager.GetEquippedPacks();
        RunConfig runConfig = new(characterProfile, DifficultyPickerView.GetSelection(), packEntries);
        AppManager.Instance.Push(AppStateMachine.RUN, runConfig);
    }
    
    private void Select(InteractBehaviour ib, PointerEventData eventData)
        => Select(ib.GetView().GetBehaviour<SelectBehaviour>());
    
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
        return CharacterListView.ViewFromIndex(index.Value).GetBehaviour<SelectBehaviour>();
    }

    private void RefreshAllSelection()
    {
        SelectBehaviour currentCharacterSelectBehaviour = GetCurrentCharacterSelectBehaviour();
        foreach (var view in CharacterListView.Traversal())
        {
            SelectBehaviour s = view.GetBehaviour<SelectBehaviour>();
            s.SetSelect(s == currentCharacterSelectBehaviour);
        }

        DetailedCharacterProfileView.SetAddress(currentCharacterSelectBehaviour.GetAddress());
        DetailedCharacterProfileView.Refresh();
    }

    private void RefreshStartRunButton()
    {
        CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;
        StartRunButton._button.interactable = characterProfile.IsUnlocked();
    }
}
