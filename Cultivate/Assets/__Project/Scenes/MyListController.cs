
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MyListController : MonoBehaviour
{
    [SerializeField] private Button _addButton;
    
    private ListModel<SkillModel> _model;
    
    public ListView SkillListView;
    
    private void Start()
    {
        _model = new ListModel<SkillModel>();
        
        _model.Add(new("一"));
        _model.Add(new("二"));
        _model.Add(new("三"));
        
        Address.AddToRoot("SkillList", _model);
        
        SkillListView.SetAddress("SkillList");
        
        _addButton.onClick.RemoveAllListeners();
        _addButton.onClick.AddListener(AddItemProcedure);
    }

    public void AddItemProcedure()
    {
        SkillModel item = new("四");
        _model.Add(item);
        SkillListView.AddItem(item);
        // AddItemStaging().GetAwaiter().GetResult();
    }

    public async UniTask AddItemStaging()
    {
        
    }
}
