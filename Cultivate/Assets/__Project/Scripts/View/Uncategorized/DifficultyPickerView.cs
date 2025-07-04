
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPickerView : MonoBehaviour
{
    [SerializeField] private TMP_Text DifficultyText;
    [SerializeField] private TMP_Text DifficultyDescriptionText;
    [SerializeField] private Button PrevDifficultyButton;
    [SerializeField] private Button NextDifficultyButton;
    [SerializeField] private PropagatePointerEnter PrevButtonPropagatePointerEnter;
    [SerializeField] private PropagatePointerEnter NextButtonPropagatePointerEnter;

    [SerializeField] private Image DemoSticker;

    private int _selectionIndex;

    public DifficultyProfile GetSelection()
        => _address.Get<DifficultyProfileList>()[_selectionIndex];

    private Address _address;

    public void Configure()
    {
        _address = new Address("Profile.ProfileList.Current.DifficultyProfileList");

        DifficultyProfileList profiles = _address.Get<DifficultyProfileList>();

        for (int i = profiles.Count() - 1; i >= 0; i--)
        {
            var p = profiles[i];
            if (p.IsUnlocked())
            {
                _selectionIndex = i;
                break;
            }
        }

        // _selectionIndex = profiles.LastIdx(p => p.IsUnlocked()).Value;

        PrevDifficultyButton.onClick.RemoveAllListeners();
        PrevDifficultyButton.onClick.AddListener(PrevDifficulty);
        PrevDifficultyButton.onClick.AddListener(AudioManager.PlayButtonPress);

        NextDifficultyButton.onClick.RemoveAllListeners();
        NextDifficultyButton.onClick.AddListener(NextDifficulty);
        NextDifficultyButton.onClick.AddListener(AudioManager.PlayButtonPress);

        PrevButtonPropagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        
        NextButtonPropagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;

        Refresh();
    }

    public void Refresh()
    {
        DifficultyProfileList profiles = _address.Get<DifficultyProfileList>();

        DifficultyProfile curr = profiles[_selectionIndex];

        DifficultyText.text = curr.GetEntry().GetName();
        DifficultyDescriptionText.text = curr.GetEntry().Description;

        int prevIndex = _selectionIndex - 1;
        bool hasPrev = new Bound(0, profiles.Count()).Contains(prevIndex);
        PrevDifficultyButton.gameObject.SetActive(hasPrev);
        if (hasPrev)
            PrevDifficultyButton.interactable = profiles[prevIndex].IsUnlocked();

        int nextIndex = _selectionIndex + 1;
        bool hasNext = new Bound(0, profiles.Count()).Contains(nextIndex);
        NextDifficultyButton.gameObject.SetActive(hasNext);
        if (hasNext)
            NextDifficultyButton.interactable = profiles[nextIndex].IsUnlocked();
        
        DemoSticker.gameObject.SetActive(curr.IsDemoLocked());
    }

    private void PrevDifficulty()
    {
        // validation
        _selectionIndex--;
        Refresh();
    }

    private void NextDifficulty()
    {
        // validation
        _selectionIndex++;
        Refresh();
    }
}
