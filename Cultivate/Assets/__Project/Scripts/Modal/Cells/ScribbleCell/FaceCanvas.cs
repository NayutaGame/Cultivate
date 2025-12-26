
using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FaceCanvas : XView
{
    private static readonly int BRUSH_POS = Shader.PropertyToID("_BrushPos");
    private static readonly int BRUSH_COLOR = Shader.PropertyToID("_BrushColor");
    private static readonly int BRUSH_SIZE = Shader.PropertyToID("_BrushSize");
    private static readonly int SCALE_X = Shader.PropertyToID("_ScaleX");
    private static readonly int SCRIBBLE_TEX = Shader.PropertyToID("_ScribbleTex");

    [SerializeField] private RectTransform Anchor;
    [NonSerialized] private PrefabEntry PrefabEntry;
    [NonSerialized] private GameObject Model;
    [NonSerialized] private SkeletonDataAsset SkeletonDataAsset;
    [NonSerialized] private Material SkeletonMat;
    
    [SerializeField] private Material PaintMat;
    [SerializeField] private RawImage DisplayImage;
    [SerializeField] private float _brushSize = 0.001f;
    [SerializeField] private Color _brushColor = Color.red;

    private RenderTexture _cacheTex;
    private RenderTexture _scribbleTex;
    private Vector2Int _textureSize;
    private SpineAttachmentRaycaster _raycaster;
    
    private bool _painting;
    private SpineAttachmentRaycaster.HitResult _lastHit;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        _painting = false;
        _lastHit = new();

        InteractBehaviour ib = GetInteractBehaviour();
        ib.NeuronBundle.PointerDownNeuron.Add(PointerDown);
        ib.NeuronBundle.PointerUpNeuron.Add(PointerUp);
        
        ib.NeuronBundle.PointerMoveNeuron.Add(PointerMove);
    }

    public void SetPrefabEntry(PrefabEntry prefabEntry)
    {
        if (PrefabEntry == prefabEntry)
            return;
        
        if (Model != null)
            Destroy(Model);

        PrefabEntry = prefabEntry;
        Model = Instantiate(prefabEntry.Prefab, Anchor);

        SkeletonGraphic skeletonGraphic = Model.GetComponentInChildren<SkeletonGraphic>();
        if (skeletonGraphic == null)
            return;
        
        _raycaster = Model.GetComponent<SpineAttachmentRaycaster>();
        SkeletonMat = skeletonGraphic.material;
        SetSkeletonDataAsset(skeletonGraphic.skeletonDataAsset);
        SkeletonMat.SetTexture(SCRIBBLE_TEX, _scribbleTex);
        
        skeletonGraphic.AnimationState.AddAnimation(1, "idle", true, 0);
    }

    private void SetSkeletonDataAsset(SkeletonDataAsset skeletonDataAsset)
    {
        ReleaseRenderTextures();
        SkeletonDataAsset = skeletonDataAsset;
        InitializeRenderTextures();
    }

    private void ReleaseRenderTextures()
    {
        if (_cacheTex != null)
        {
            _cacheTex.Release();
            Destroy(_cacheTex);
            _cacheTex = null;
        }
        
        if (_scribbleTex != null)
        {
            _scribbleTex.Release();
            Destroy(_scribbleTex);
            _scribbleTex = null;
        }
        
        _textureSize = Vector2Int.zero;
    }

    private void InitializeRenderTextures()
    {
        if (SkeletonDataAsset == null)
            return;
        
        AtlasAssetBase atlasAsset = SkeletonDataAsset.atlasAssets[0];
        Texture mainTexture = atlasAsset.PrimaryMaterial.mainTexture;
        
        _cacheTex = new RenderTexture(mainTexture.width, mainTexture.height, 0, RenderTextureFormat.ARGB32);
        _scribbleTex = new RenderTexture(mainTexture.width, mainTexture.height, 0, RenderTextureFormat.ARGB32);
        _textureSize = new Vector2Int(mainTexture.width, mainTexture.height);
    }
    
    private void PointerDown(InteractBehaviour ib, PointerEventData d)
    {
        _painting = true;
        _lastHit = new();
        
        _raycaster.Raycast(d.position, out SpineAttachmentRaycaster.HitResult currHit);
        
        DrawFromTwoHits(currHit, currHit);
        _lastHit = currHit;
    }
    
    private void PointerUp(InteractBehaviour ib, PointerEventData d)
    {
        _painting = false;
        _lastHit = new();
    }
    
    private void PointerMove(InteractBehaviour ib, PointerEventData d)
    {
        if (!_painting)
            return;
    
        _raycaster.Raycast(d.position, out SpineAttachmentRaycaster.HitResult currHit);
    
        if (_lastHit.IsValid)
        {
            DrawFromTwoHits(_lastHit, currHit);
        }
        else
        {
            DrawFromTwoHits(currHit, currHit);
        }

        _lastHit = currHit;
    }
    
    private void DrawFromTwoHits(SpineAttachmentRaycaster.HitResult hit1, SpineAttachmentRaycaster.HitResult hit2)
    {
        // case1: invalid -> normal (上一帧打空气，这一帧打中)
        // case2: normal -> invalid (上一帧打中，这一帧打空气，忽略)
        // case3: same attachment (同一 attachment，需要判断距离)
        // case4: different attachment (不同 attachment)
        
        bool hit1Valid = hit1.IsValid;
        bool hit2Valid = hit2.IsValid;
        
        // case2: 上一帧打中，这一帧打空气，直接返回不绘制
        if (hit1Valid && !hit2Valid)
        {
            return;
        }
        
        // case1: 上一帧打空气，这一帧打中，只绘制当前点
        bool case1 = !hit1Valid && hit2Valid;
        
        // case3 和 case4: 需要判断是否为同一 attachment
        bool case3 = false;
        bool case4 = false;
        
        if (hit1Valid && hit2Valid)
        {
            // 判断是否为同一 attachment
            bool sameAttachment = hit1.Attachment == hit2.Attachment;
            
            if (sameAttachment)
            {
                case3 = true;
            }
            else
            {
                case4 = true;
            }
        }

        if (case1 || case4)
        {
            // 只绘制当前点，不连线
            DrawBrush(hit2.SpineUV.x, hit2.SpineUV.y);
            return;
        }

        if (case3)
        {
            // 同一 attachment 且距离合理，正常连线
            DrawLine(hit1.SpineUV.x, hit1.SpineUV.y, hit2.SpineUV.x, hit2.SpineUV.y);
            return;
        }
    }

    private void DrawLine(float x1, float y1, float x2, float y2)
    {
        // 步长：使用笔刷大小的一半，确保笔触之间有重叠
        float step = _brushSize * 0.5f;
        
        // 计算两点之间的距离
        float distance = Mathf.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        
        // 如果距离太小，直接绘制一个点
        if (distance < step)
        {
            DrawBrush(x1, y1);
            return;
        }
        
        // 计算需要的步数
        int stepCount = Mathf.CeilToInt(distance / step);
        
        // 使用线性插值在两点之间绘制
        for (int i = 0; i <= stepCount; i++)
        {
            // t 从 0 到 1，包含起点和终点
            float t = i / (float)stepCount;
            float x = Mathf.Lerp(x1, x2, t);
            float y = Mathf.Lerp(y1, y2, t);
            DrawBrush(x, y);
        }
    }
    
    private void DrawBrush(float x, float y)
    {
        PaintMat.SetVector(BRUSH_POS, new Vector4(x, y, 0, 0));
        PaintMat.SetColor(BRUSH_COLOR, _brushColor);
        PaintMat.SetFloat(BRUSH_SIZE, _brushSize);
        PaintMat.SetFloat(SCALE_X, (float)_textureSize.x / _textureSize.y);
        
        Graphics.Blit(_scribbleTex, _cacheTex, PaintMat, 0);
        Graphics.Blit(_cacheTex, _scribbleTex);
    }

    private void GetTexture2D(ref Texture2D tex)
    {
        // Texture2D texture2D = new Texture2D(CanvasSize.x, CanvasSize.y, TextureFormat.ARGB32, false);
        
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = _scribbleTex;
        tex.ReadPixels(new Rect(0, 0, _textureSize.x, _textureSize.y), 0, 0);
        tex.Apply();
        RenderTexture.active = previous;
    }

    private void ClearScribble()
    {
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = _scribbleTex;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = previous;
    }

    private void OnDestroy()
    {
        DisplayImage.texture = null;
    }
}