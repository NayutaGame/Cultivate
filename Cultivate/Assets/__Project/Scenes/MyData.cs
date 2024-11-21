
using UnityEngine;

public class MyData : MonoBehaviour
{
    private Encyclopedia Encyclopedia;
    private ListModel<SimpleItem> Inventory;

    public NewListView NewListView;
    
    void Awake()
    {
        Encyclopedia = new();
        Inventory = new();
        Inventory.Add(new("一"));
        Inventory.Add(new("二"));
        Inventory.Add(new("三"));
        
        Address.SetValueOnRoot("SkillInventory", Inventory);
        
        NewListView.SetAddress("SkillInventory");
    }
}
