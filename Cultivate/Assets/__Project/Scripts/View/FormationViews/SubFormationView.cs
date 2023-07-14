using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubFormationView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text ConditionText;
    [SerializeField] private TMP_Text RewardText;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh()
    {
        SubFormationEntry e = DataManager.Get<SubFormationEntry>(GetIndexPath());
        if (e == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        JingJieText.text = e.GetJingJie().ToString();
        ConditionText.text = e.GetConditionDescription();
        RewardText.text = e.GetRewardDescription();
    }
}
