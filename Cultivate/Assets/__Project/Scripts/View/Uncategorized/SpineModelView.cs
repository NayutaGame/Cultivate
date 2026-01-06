
using System;
using Spine.Unity;
using UnityEngine;

public class SpineModelView : MonoBehaviour
{
    [SerializeField] private RectTransform Anchor;
    [NonSerialized] private PrefabEntry PrefabEntry;
    [NonSerialized] private GameObject Model;

    private void OnDisable()
    {
        SetPrefabEntry(null);
    }

    public void SetPrefabEntry(PrefabEntry prefabEntry)
    {
        if (PrefabEntry == prefabEntry)
            return;
        
        if (Model != null)
            Destroy(Model);

        if (prefabEntry == null)
            return;

        PrefabEntry = prefabEntry;
        Model = Instantiate(prefabEntry.Prefab, Anchor);

        SkeletonGraphic skeletonGraphic = Model.GetComponentInChildren<SkeletonGraphic>();
        if (skeletonGraphic != null)
        {
            skeletonGraphic.AnimationState.SetAnimation(1, "idle", true);
        }
    }
}
