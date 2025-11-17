
using UnityEngine;
using Spine;
using Spine.Unity;

/// <summary>
/// 通过射线检测找到被点击的Spine Attachment
/// 按照绘制顺序（DrawOrder）从后往前检测，返回第一个被击中的attachment
/// </summary>
public class SpineAttachmentRaycaster : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic SkeletonGraphic;
    [SerializeField] private RectTransform SkeletonRect;
    
    private float[] _worldVerticesCache = new float[8];
    private Vector2[] _quadVertices = new Vector2[4];
    
    public struct HitResult
    {
        public Slot Slot;                             // 被击中的slot
        public Attachment Attachment;                 // 被击中的attachment
        public RegionAttachment RegionAttachment;     // 如果是RegionAttachment，这里不为null
        public MeshAttachment MeshAttachment;         // 如果是MeshAttachment，这里不为null
        public Vector2 SpineUV;                       // 在Spine Atlas中的UV坐标 (0-1)
        public Vector2 LocalPos;                      // 在attachment中的局部坐标 (0-1)
        
        // RegionAttachment: 4个顶点的世界坐标和UV (BL, UL, UR, BR)
        public Vector2[] RegionWorldVertices;         // 4个顶点的世界坐标
        public Vector2[] RegionUVs;                   // 4个顶点对应的UV坐标
        
        // MeshAttachment: 3个顶点的世界坐标和UV (三角形)
        public Vector2[] MeshWorldVertices;          // 3个顶点的世界坐标
        public Vector2[] MeshUVs;                     // 3个顶点对应的UV坐标
        public Vector3 BarycentricCoordinates;       // 重心坐标 (用于MeshAttachment)
        
        public bool IsValid => Slot != null && Attachment != null;
    }
    
    /// <summary>
    /// 从屏幕坐标发出射线，检测击中的第一个attachment
    /// </summary>
    /// <param name="screenPos">屏幕坐标</param>
    /// <param name="result">检测结果</param>
    /// <returns>是否击中attachment</returns>
    public bool Raycast(Vector2 screenPos, out HitResult result)
    {
        Skeleton skeleton = SkeletonGraphic.Skeleton;
        
        // 1. 将屏幕坐标转换为Skeleton空间坐标
        Vector2 skeletonSpacePos = ScreenToSkeletonSpace(screenPos);
        
        // 2. 按照DrawOrder从后往前遍历（后面的先绘制，前面的后绘制，所以前面的在上层）
        // 我们需要从前往后遍历，找到第一个（最上层的）被击中的
        ExposedList<Slot> drawOrder = skeleton.DrawOrder;
        Slot[] drawOrderItems = drawOrder.Items;
        
        // 从后往前遍历（最上层的attachment在最后）
        for (int i = drawOrder.Count - 1; i >= 0; i--)
        {
            Slot slot = drawOrderItems[i];
            
            // 跳过无效的slot
            if (slot == null || !slot.Bone.Active || slot.A == 0f)
                continue;
            
            Attachment attachment = slot.Attachment;
            if (attachment == null)
                continue;
            
            if (attachment is RegionAttachment regionAttachment)
            {
                if (IsPointInRegionAttachment(skeletonSpacePos, slot, regionAttachment, out result))
                {
                    return true;
                }
                continue;
            }
            
            if (attachment is MeshAttachment meshAttachment)
            {
                if (IsPointInMeshAttachment(skeletonSpacePos, slot, meshAttachment, out result))
                {
                    return true;
                }
                continue;
            }
        }
        
        result = new HitResult();
        return false;
    }
    
    /// <summary>
    /// 将屏幕坐标转换为Skeleton空间坐标
    /// </summary>
    private Vector2 ScreenToSkeletonSpace(Vector2 screenPos)
    {
        // 1. 屏幕坐标 -> Spine RectTransform本地坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(SkeletonRect, screenPos, CameraManager.Instance.GetCamera(), out Vector2 spineLocalPos);
        
        // 2. 转换为Skeleton空间坐标
        float meshScale = SkeletonGraphic.MeshScale;
        Skeleton skeleton = SkeletonGraphic.Skeleton;
        Vector2 skeletonSpacePos = spineLocalPos / meshScale;
        skeletonSpacePos.x /= skeleton.ScaleX;
        skeletonSpacePos.y /= skeleton.ScaleY;
        
        return skeletonSpacePos;
    }
    
    /// <summary>
    /// 检测点是否在RegionAttachment内
    /// </summary>
    private bool IsPointInRegionAttachment(Vector2 point, Slot slot, RegionAttachment attachment, 
        out HitResult result)
    {
        result = new HitResult();
        
        // 计算attachment的4个世界坐标顶点
        attachment.ComputeWorldVertices(slot, _worldVerticesCache, 0);
        
        // 转换为Vector2数组
        _quadVertices[0] = new Vector2(_worldVerticesCache[0], _worldVerticesCache[1]); // BL
        _quadVertices[1] = new Vector2(_worldVerticesCache[2], _worldVerticesCache[3]); // UL
        _quadVertices[2] = new Vector2(_worldVerticesCache[4], _worldVerticesCache[5]); // UR
        _quadVertices[3] = new Vector2(_worldVerticesCache[6], _worldVerticesCache[7]); // BR
        
        // 判断点是否在四边形内
        if (!IsPointInQuad(point, _quadVertices))
            return false;
        
        // 计算局部坐标
        Vector2 localPos = GetLocalPositionInQuad(point, _quadVertices);
        
        // 转换为Spine Atlas UV
        float[] uvs = attachment.UVs;
        Vector2 spineUV = BilinearInterpolateUV(localPos, uvs);
        
        // 存储4个顶点的世界坐标和UV
        Vector2[] regionWorldVertices = new Vector2[4]
        {
            _quadVertices[0], // BL
            _quadVertices[1], // UL
            _quadVertices[2], // UR
            _quadVertices[3]  // BR
        };
        
        Vector2[] regionUVs = new Vector2[4]
        {
            new Vector2(uvs[0], uvs[1]), // BL
            new Vector2(uvs[2], uvs[3]), // UL
            new Vector2(uvs[4], uvs[5]), // UR
            new Vector2(uvs[6], uvs[7])  // BR
        };
        
        result = new HitResult
        {
            Slot = slot,
            Attachment = attachment,
            RegionAttachment = attachment,
            SpineUV = spineUV,
            LocalPos = localPos,
            RegionWorldVertices = regionWorldVertices,
            RegionUVs = regionUVs
        };
        
        return true;
    }
    
    /// <summary>
    /// 检测点是否在MeshAttachment内
    /// </summary>
    private bool IsPointInMeshAttachment(Vector2 point, Slot slot, MeshAttachment attachment,
        out HitResult result)
    {
        result = new HitResult();
        
        // 计算mesh的所有世界坐标顶点
        int vertexCount = attachment.WorldVerticesLength >> 1; // 除以2，因为每个顶点有x,y两个值
        if (_worldVerticesCache.Length < attachment.WorldVerticesLength)
        {
            _worldVerticesCache = new float[attachment.WorldVerticesLength];
        }
        
        attachment.ComputeWorldVertices(slot, 0, attachment.WorldVerticesLength, _worldVerticesCache, 0);
        
        // 获取三角形索引
        int[] triangles = attachment.Triangles;
        
        // 遍历所有三角形，判断点是否在其中
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i0 = triangles[i] * 2;
            int i1 = triangles[i + 1] * 2;
            int i2 = triangles[i + 2] * 2;
            
            Vector2 v0 = new Vector2(_worldVerticesCache[i0], _worldVerticesCache[i0 + 1]);
            Vector2 v1 = new Vector2(_worldVerticesCache[i1], _worldVerticesCache[i1 + 1]);
            Vector2 v2 = new Vector2(_worldVerticesCache[i2], _worldVerticesCache[i2 + 1]);
            
            if (IsPointInTriangle(point, v0, v1, v2))
            {
                // 计算重心坐标
                Vector3 barycentric = GetBarycentricCoordinates(point, v0, v1, v2);
                
                // 使用重心坐标插值计算UV
                float[] uvs = attachment.UVs;
                Vector2 uv0 = new Vector2(uvs[i0], uvs[i0 + 1]);
                Vector2 uv1 = new Vector2(uvs[i1], uvs[i1 + 1]);
                Vector2 uv2 = new Vector2(uvs[i2], uvs[i2 + 1]);
                
                Vector2 spineUV = barycentric.x * uv0 + barycentric.y * uv1 + barycentric.z * uv2;
                
                // 计算局部坐标（相对于三角形的边界框）
                Vector2 localPos = barycentric;
                
                // 存储3个顶点的世界坐标和UV
                Vector2[] meshWorldVertices = new Vector2[3] { v0, v1, v2 };
                Vector2[] meshUVs = new Vector2[3] { uv0, uv1, uv2 };
                
                result = new HitResult
                {
                    Slot = slot,
                    Attachment = attachment,
                    MeshAttachment = attachment,
                    SpineUV = spineUV,
                    LocalPos = localPos,
                    MeshWorldVertices = meshWorldVertices,
                    MeshUVs = meshUVs,
                    BarycentricCoordinates = barycentric
                };
                
                return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// 判断点是否在四边形内
    /// </summary>
    private bool IsPointInQuad(Vector2 point, Vector2[] quadVertices)
    {
        // 将四边形分成两个三角形
        return IsPointInTriangle(point, quadVertices[0], quadVertices[1], quadVertices[2]) ||
               IsPointInTriangle(point, quadVertices[0], quadVertices[2], quadVertices[3]);
    }
    
    /// <summary>
    /// 判断点是否在三角形内（使用重心坐标法）
    /// </summary>
    private bool IsPointInTriangle(Vector2 point, Vector2 v0, Vector2 v1, Vector2 v2)
    {
        Vector3 barycentric = GetBarycentricCoordinates(point, v0, v1, v2);
        return barycentric.x >= 0 && barycentric.y >= 0 && barycentric.z >= 0;
    }
    
    /// <summary>
    /// 获取点在三角形中的重心坐标
    /// </summary>
    private Vector3 GetBarycentricCoordinates(Vector2 point, Vector2 v0, Vector2 v1, Vector2 v2)
    {
        Vector2 v0v1 = v1 - v0;
        Vector2 v0v2 = v2 - v0;
        Vector2 v0p = point - v0;
        
        float dot00 = Vector2.Dot(v0v2, v0v2);
        float dot01 = Vector2.Dot(v0v2, v0v1);
        float dot02 = Vector2.Dot(v0v2, v0p);
        float dot11 = Vector2.Dot(v0v1, v0v1);
        float dot12 = Vector2.Dot(v0v1, v0p);
        
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float v = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float u = (dot00 * dot12 - dot01 * dot02) * invDenom;
        float w = 1 - u - v;
        
        return new Vector3(w, u, v); // w对应v0, u对应v1, v对应v2
    }
    
    /// <summary>
    /// 获取点在四边形中的局部坐标（归一化到0-1）
    /// </summary>
    private Vector2 GetLocalPositionInQuad(Vector2 point, Vector2[] quadVertices)
    {
        Vector2 bl = quadVertices[0];
        Vector2 ul = quadVertices[1];
        Vector2 ur = quadVertices[2];
        Vector2 br = quadVertices[3];
        
        // 初始猜测（使用边界框投影）
        float minX = Mathf.Min(bl.x, ul.x, ur.x, br.x);
        float maxX = Mathf.Max(bl.x, ul.x, ur.x, br.x);
        float minY = Mathf.Min(bl.y, ul.y, ur.y, br.y);
        float maxY = Mathf.Max(bl.y, ul.y, ur.y, br.y);
        
        float u = (point.x - minX) / (maxX - minX);
        float v = (point.y - minY) / (maxY - minY);
        
        // 使用牛顿迭代法优化
        for (int i = 0; i < 3; i++)
        {
            Vector2 p00 = bl;
            Vector2 p10 = br;
            Vector2 p01 = ul;
            Vector2 p11 = ur;
            
            Vector2 estimated = (1 - u) * (1 - v) * p00 + u * (1 - v) * p10 + (1 - u) * v * p01 + u * v * p11;
            Vector2 error = point - estimated;
            
            Vector2 du = (1 - v) * (p10 - p00) + v * (p11 - p01);
            Vector2 dv = (1 - u) * (p01 - p00) + u * (p11 - p10);
            
            float a = Vector2.Dot(du, du);
            float b = Vector2.Dot(du, dv);
            float c = Vector2.Dot(dv, dv);
            float d = Vector2.Dot(error, du);
            float e = Vector2.Dot(error, dv);
            
            float det = a * c - b * b;
            if (Mathf.Abs(det) < 0.0001f)
                break;
            
            float deltaU = (c * d - b * e) / det;
            float deltaV = (a * e - b * d) / det;
            
            u = Mathf.Clamp01(u + deltaU);
            v = Mathf.Clamp01(v + deltaV);
            
            if (Mathf.Abs(deltaU) < 0.001f && Mathf.Abs(deltaV) < 0.001f)
                break;
        }
        
        return new Vector2(u, v);
    }
    
    /// <summary>
    /// 使用双线性插值计算UV坐标
    /// </summary>
    private Vector2 BilinearInterpolateUV(Vector2 localPos, float[] uvs)
    {
        // RegionAttachment的UV顺序: BL, UL, UR, BR
        Vector2 bl = new Vector2(uvs[0], uvs[1]);
        Vector2 ul = new Vector2(uvs[2], uvs[3]);
        Vector2 ur = new Vector2(uvs[4], uvs[5]);
        Vector2 br = new Vector2(uvs[6], uvs[7]);
        
        float u = localPos.x;
        float v = localPos.y;
        
        Vector2 bottom = Vector2.Lerp(bl, br, u);
        Vector2 top = Vector2.Lerp(ul, ur, u);
        Vector2 result = Vector2.Lerp(bottom, top, v);
        
        return result;
    }
}

