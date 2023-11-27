
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Matrix3x2 = System.Numerics.Matrix3x2;

public class AnimatedListView : ListView, IDropHandler, IPointerMoveHandler
{
    // [NonSerialized] private static readonly Matrix4x4 Mat = new Matrix4x4(
    //     new Vector4(0.0004409171f, 0.00000f, 0.00000f, 0.0f),
    //     new Vector4(0.00000f, 0.002083333f, 0.00000f, 0.0f),
    //     new Vector4(0.00000f, 0.00000f, 1.00000f, 0.00000f),
    //     new Vector4(-0.3465608f, 0.12500f, 0.00000f, 1.00000f));
    // [NonSerialized] private static readonly Matrix4x4 MatInv = new Matrix4x4(
    //     new Vector4(2268.00000f, 0.00000f, 0.00000f, 0.0f),
    //     new Vector4(0.00000f, 480.00000f, 0.00000f, 0.0f),
    //     new Vector4(0.00000f, 0.00000f, 1.00000f, 0.00000f),
    //     new Vector4(786.00000f, -60.00000f, 0.00000f, 1.00000f));

    [NonSerialized] private static readonly Matrix3x2 M = new Matrix3x2(0.0004409171f, 0f, 0f, 0.002083333f, -0.3465608f, 0.12500f);
    [NonSerialized] private static readonly Matrix3x2 MInv = new Matrix3x2(2268.00000f, 0f, 0f, 480.00000f, 786.00000f, -60.00000f);

    [SerializeField] private RectTransform RealZone;
    [SerializeField] private GameObject PivotPrefab;

    public override void Sync()
    {
        PutAllIntoPool();

        for (int i = 0; i < Model.Count(); i++)
        {
            Address address = GetAddress().Append($"#{i}");
            int prefabIndex = GetPrefabIndex(address.Get<object>());
            FetchItemView(out ItemView itemView, prefabIndex);
            EnableItemView(itemView, i);
            itemView.SetAddress(address);
        }

        Refresh();
    }

    protected override Task InsertItem(int index, object item)
    {
        Task task = base.InsertItem(index, item);
        // RefreshPivots();
        return task;
    }

    protected override Task RemoveAt(int index)
    {
        var task = base.RemoveAt(index);
        // RefreshPivots();
        return task;
    }

    protected override Task Modified(int index)
    {
        return base.Modified(index);
    }

    protected override void BindItemView(ItemView itemView, int prefabIndex = 0)
    {
        base.BindItemView(itemView, prefabIndex);

        if (itemView is HandSkillView handSkillView)
        {
            if (handSkillView.PivotPropagate != null)
                return;

            PivotPropagate pivotPropagate = Instantiate(PivotPrefab, RealZone).GetComponent<PivotPropagate>();
            handSkillView.PivotPropagate = pivotPropagate;
            pivotPropagate.BindingView = handSkillView.GetComponent<IInteractable>();
        }
    }

    public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);

    public void OnPointerMove(PointerEventData eventData)
    {
        RefreshPivots();
    }

    public void RefreshPivots()
    {
        float m = Mathf.Clamp01(MultiplyX(M, Input.mousePosition.x));
        // float m2 = GetComponent<RectTransform>().InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x;
        // Debug.Log($"m = {m}\nm2 = {m2}");

        float L = 1134;
        float D = 600;
        float S = 300;
        float c = _activePool.Count * D / L; // count contained in handle space
        float p = Mathf.Min(-Mathf.Log(S / D) / Mathf.Log(c), 1);

        // TODO: ignore dragging element

        float hoverS = 1.5f;
        float hoverY = 80;

        for (int i = 0; i < _activePool.Count; i++)
        {
            HandSkillView handSkillView = _activePool[i] as HandSkillView;
            if (handSkillView == null)
                continue;

            float d_nPS = (float)i / (_activePool.Count - 1) - m; // dist in normalized port space
            float d_nHS = d_nPS / D * L * 2; // dist in normalized handle space
            float x = -L / 2 + (i + 0.5f) / _activePool.Count * L; // position in port space, before magnifier

            float v = Mathf.Clamp(d_nHS, -1, 1);
            float w = v * v;

            float y = (1 - w) * hoverY;

            float s = (1 - w) * hoverS + w;

            if (d_nHS is > -1 and < 1)
                x += (Mathf.Sign(d_nHS) * Mathf.Pow(Mathf.Abs(d_nHS), p) - d_nHS) * D / 2;

            RectTransform t = handSkillView.PivotPropagate.RectTransform;
            t.anchoredPosition = new Vector2(x, t.anchoredPosition.y);

            RectTransform idle = handSkillView.PivotPropagate.IdlePivot;
            idle.anchoredPosition = new Vector2(idle.anchoredPosition.x, y);
            idle.localScale = s * Vector3.one;

            handSkillView.GoToPivot(handSkillView.PivotPropagate.IdlePivot);
        }
    }

    private static float MultiplyX(Matrix3x2 m, float x)
        => m.M11 * x + m.M31;

    private static Vector2 Multiply(Matrix3x2 m, Vector2 p)
        => new Vector2(m.M11 * p.x + m.M21 * p.y + m.M31, m.M12 * p.x + m.M22 * p.y + m.M32);

    // private void Update()
    // {
    //     if (!Input.GetKeyDown(KeyCode.P))
    //         return;
    //
    //     // CalcMat();
    //
    //     Vector2 p = Input.mousePosition;
    //     Vector2 q = Multiply(Mat32, p);
    //     Vector2 pStar = Multiply(Mat32Inv, q);
    //
    //     Debug.Log(p);
    //     Debug.Log(q);
    //     Debug.Log(pStar);
    //
    //     // Debug.Log(vStar);
    //     // Debug.Log($"{Mat.MultiplyPoint(Input.mousePosition)}");
    // }
    //
    // private void CalcMat()
    // {
    //     Vector3[] fourCorners = new Vector3[4];
    //     RealZone.GetWorldCorners(fourCorners);
    //
    //     Vector3 p00 = fourCorners[0];
    //     Vector3 p11 = fourCorners[2];
    //     Vector3 diff = p11 - p00;
    //
    //     Matrix4x4 m = Matrix4x4.TRS(p00, Quaternion.identity, new Vector3(diff.x, diff.y, 1)).inverse;
    //     Matrix4x4 mInv = Matrix4x4.TRS(p00, Quaternion.identity, new Vector3(diff.x, diff.y, 1));
    //
    //     // Vector3 p = Input.mousePosition;
    //     // Vector3 q = m.MultiplyPoint(p);
    //     // Vector3 pStar = mInv.MultiplyPoint(q);
    //     //
    //     // Debug.Log(p);
    //     // Debug.Log(q);
    //     // Debug.Log(pStar);
    //
    //     // CheckResult(p00, p11, m);
    // }
    //
    // private void CheckResult(Vector3 p00, Vector3 p11, Matrix4x4 m)
    // {
    //     Debug.Log($"p00 => {m.MultiplyPoint(p00)}");
    //     Debug.Log($"p11 => {m.MultiplyPoint(p11)}");
    //     Debug.Log($"mouse => {m.MultiplyPoint(Input.mousePosition)}");
    // }
}
