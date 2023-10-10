
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Button ReturnButton;
    [SerializeField] private ListView TutorialListView;

    public void Configure()
    {
        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(CloseTutorial);
    }

    public void Refresh()
    {

    }

    private void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}
