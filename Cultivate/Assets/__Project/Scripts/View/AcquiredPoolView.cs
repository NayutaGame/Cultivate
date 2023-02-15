using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquiredPoolView : MonoBehaviour
{
    public Transform ContentTransform;
    public GameObject AcquiredChipPrefab;

    public void Configure()
    {
        PopulateList();
    }

    private void PopulateList()
    {
        int current = ContentTransform.childCount;
        int need = RunManager.Get<int>("GetAcquiredChipCount");

        if (need > current)
        {
            int compensate = need - current;
            for (int i = 0; i < compensate; i++)
            {
                Instantiate(AcquiredChipPrefab, ContentTransform);
            }
        }

        for (int i = 0; i < ContentTransform.childCount; i++)
        {
            RunChipView v = ContentTransform.GetChild(i).GetComponent<RunChipView>();
            v.Configure(new IndexPath("TryGetAcquiredChip", i));
        }
    }
}
