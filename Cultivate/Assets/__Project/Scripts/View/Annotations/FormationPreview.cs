
using UnityEngine;

// TODO: to be removed
public class FormationPreview : FormationView
{
    public override void Refresh()
    {
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        base.Refresh();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform rectTransform = GetDisplayTransform();
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }
}
