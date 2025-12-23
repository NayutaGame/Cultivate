
using TMPro;
using UnityEngine;

public class ListViewWithSearchBar : XView
{
    [SerializeField] private TMP_InputField SearchBar;
    [SerializeField] public ListView Browser;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        SearchBar.onEndEdit.RemoveAllListeners();
        SearchBar.onEndEdit.AddListener(OnSearchBarValueChanged);
        
        Browser.SetAddress(GetAddress().Append(".Browser"));
    }

    private void OnSearchBarValueChanged(string value)
    {
        IListModel<ISearchable> list = Get<IListModel<ISearchable>>();
        list.SetSearchText(value);
        Browser.Sync();
    }
}
