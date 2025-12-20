
using UnityEngine;
using UnityEngine.UI;

public class ListViewWithScroll : MonoBehaviour
{
    [SerializeField] private ListView ListView;
    [SerializeField] private ScrollRect ScrollRect;

    private void Awake()
    {
        ScrollRect.onValueChanged.RemoveAllListeners();
        ScrollRect.onValueChanged.AddListener(RefreshPivots);
    }

    private void RefreshPivots(Vector2 value)
    {
        ListView.RefreshPivots();
    }
}
