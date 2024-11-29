
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MyAnimatedListController : MonoBehaviour
{
    [SerializeField] private Button _addButton;
    [SerializeField] private PinAnchor _pinAnchor;
    
    private ListModel<SkillModel> _model;
    
    public AnimatedListView SkillAnimatedListView;
    
    private void Start()
    {
        _model = new ListModel<SkillModel>();
        
        _model.Add(new("一"));
        _model.Add(new("二"));
        _model.Add(new("三"));
        
        Address.AddToRoot("AnimatedSkillList", _model);
        
        SkillAnimatedListView.SetAddress("AnimatedSkillList");
        
        _addButton.onClick.RemoveAllListeners();
        _addButton.onClick.AddListener(AddItemProcedure);
    }

    public void AddItemProcedure()
    {
        SkillModel item = new("四");
        _model.Add(item);
        SkillAnimatedListView.AddItem(item);
        // AddItemStaging().GetAwaiter().GetResult();
    }

    public async UniTask AddItemStaging()
    {
        
    }
}
