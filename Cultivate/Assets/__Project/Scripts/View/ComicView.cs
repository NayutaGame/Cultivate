
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ComicView : MonoBehaviour
{
    [SerializeField] private Image[] Pages;

    private int _index;

    public void Init()
    {
        Pages.Do(image => image.color = new Color(1, 1, 1, 0));
        _index = 0;

        Click();
    }

    private Tween _handle;
    
    public bool Click()
    {
        if (Pages.Length <= _index)
            return true;

        _handle?.Complete();
        _handle = Pages[_index].GetComponent<Image>().DOFade(1, 0.15f);
        _handle.SetAutoKill().Restart();

        _index++;
        
        return false;
    }
}
