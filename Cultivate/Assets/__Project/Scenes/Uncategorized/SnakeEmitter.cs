using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SnakeEmitter : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;        // 移动速度
    public float turnDelay = 0.5f;      // 转弯时的延迟时间

    [Header("边界设置")]
    public Bounds moveBounds;           // 移动边界

    private Vector3 currentPosition;    // 当前位置
    private Vector3 targetPosition;     // 目标位置
    private List<Vector3> pathPoints;   // 路径点列表
    private int currentPathIndex;       // 当前路径点索引
    private float turnTimer;            // 转弯计时器
    private bool isTurning;             // 是否正在转弯

    private Coroutine _coroutine;

    [SerializeField] private ParticleSystem _particleSystem;

    private void Start()
    {
        // 初始化路径点列表
        pathPoints = new List<Vector3>();
        _particleSystem.Stop();
        // 生成新的路径
        GenerateNewPath();
    }

    private void Update()
    {
        if (pathPoints.Count == 0) return;

        if (isTurning)
        {
            // 处理转弯延迟
            turnTimer += Time.deltaTime;
            if (turnTimer >= turnDelay)
            {
                isTurning = false;
                turnTimer = 0f;
            }
            return;
        }

        // 移动到下一个路径点
        Vector3 direction = (pathPoints[currentPathIndex] - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 检查是否到达当前路径点
        if (Vector3.Distance(transform.position, pathPoints[currentPathIndex]) < 0.1f)
        {
            currentPathIndex++;
            isTurning = true;

            // 如果到达路径终点，生成新路径
            if (currentPathIndex >= pathPoints.Count)
            {
                isTurning = false;
                GenerateNewPath();
            }
        }
    }

    private void GenerateNewPath()
    {
        // 清理记录的coroutine
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        pathPoints.Clear();
        currentPathIndex = 0;

        int[] selections = RandomSelect(4, 2);

        Vector3 startPoint = RandomPointOnEdge(selections[0]);
        pathPoints.Add(startPoint);
        transform.position = startPoint;

        Vector3 endPoint = RandomPointOnEdge(selections[1]);

        bool isOpposite = selections[0] - selections[1] == 2 || selections[0] - selections[1] == -2;

        if (isOpposite)
        {
            float t = Random.value;
            t = t * 0.7f + 0.15f;

            bool horizontal = selections[0] + selections[1] == 4;
            if (horizontal)
            {
                Vector3 p1 = new Vector3(Mathf.Lerp(moveBounds.min.x, moveBounds.max.x, t), startPoint.y, 0);
                Vector3 p2 = new Vector3(Mathf.Lerp(moveBounds.min.x, moveBounds.max.x, t), endPoint.y, 0);
                pathPoints.Add(p1);
                pathPoints.Add(p2);
            }
            else
            {
                Vector3 p1 = new Vector3(startPoint.x, Mathf.Lerp(moveBounds.min.y, moveBounds.max.y, t), 0);
                Vector3 p2 = new Vector3(endPoint.x, Mathf.Lerp(moveBounds.min.y, moveBounds.max.y, t), 0);
                pathPoints.Add(p1);
                pathPoints.Add(p2);
            }
        }
        else
        {
            bool horizontalFirst = selections[0] == 1 || selections[0] == 3;
            if (horizontalFirst)
            {
                Vector3 p1 = new Vector3(endPoint.x, startPoint.y, 0);
                pathPoints.Add(p1);
            }
            else
            {
                Vector3 p1 = new Vector3(startPoint.x, endPoint.y, 0);
                pathPoints.Add(p1);
            }
        }
        
        pathPoints.Add(endPoint);

        // 制作新的coroutine
        _coroutine = StartCoroutine(ParticleCoroutine());
    }

    private IEnumerator ParticleCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 5f));

        _particleSystem.Play();

        yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));

        _particleSystem.Stop();
    }

    private int[] RandomSelect(int total, int count)
    {
        List<int> pool = new List<int>();
        for (int i = 0; i < total; i++)
        {
            pool.Add(i);
        }

        int[] result = new int[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            result[i] = pool[index];
            pool.RemoveAt(index);
        }

        return result;
    }

    private Vector3 RandomPointOnEdge(int side)
    {
        float x, y;
        float t = Random.value;
        // map t to [0.15, 0.85]
        t = t * 0.7f + 0.15f;

        switch (side)
        {
            case 0: // 上边
                x = Mathf.Lerp(moveBounds.min.x, moveBounds.max.x, t);
                y = moveBounds.max.y;
                break;
            case 1: // 右边
                x = moveBounds.max.x;
                y = Mathf.Lerp(moveBounds.min.y, moveBounds.max.y, t);
                break;
            case 2: // 下边
                x = Mathf.Lerp(moveBounds.min.x, moveBounds.max.x, t);
                y = moveBounds.min.y;
                break;
            default: // 左边
                x = moveBounds.min.x;
                y = Mathf.Lerp(moveBounds.min.y, moveBounds.max.y, t);
                break;
        }

        return new Vector3(x, y, 0);
    }

    // 在Scene视图中绘制路径（调试用）
    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
        }

        // 绘制边界
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(moveBounds.center, moveBounds.size);
    }
}