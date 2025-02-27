using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class InfiniteScrollView : MonoBehaviour
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }

    [Header("Base Settings")]
    public Direction direction = Direction.Vertical;
    public ScrollRect scrollRect;
    public RectTransform viewport;
    public RectTransform content;
    public GameObject cellPrefab;
    public float spacing = 10f;

    [Header("Jump Settings")]
    public float animationDuration = 0.5f;
    public Ease animationEase = Ease.OutQuad;

    private List<RectTransform> activeCells = new List<RectTransform>();
    private Queue<RectTransform> cellPool = new Queue<RectTransform>();
    private float cellSize;
    private int dataCount;
    private int currentFirstIndex;
    private Vector2 contentStartPos;

    void Start()
    {
        Initialize();
        StartCoroutine(SetupLayout());
    }

    void Initialize()
    {
        scrollRect.onValueChanged.AddListener(OnScroll);
        cellSize = direction == Direction.Vertical ? 
            cellPrefab.GetComponent<RectTransform>().rect.height : 
            cellPrefab.GetComponent<RectTransform>().rect.width;
        contentStartPos = content.anchoredPosition;
    }

    IEnumerator SetupLayout()
    {
        yield return null; // Wait for layout rebuild
        
        // 初始化内容尺寸
        dataCount = 1000; // 示例数据量
        UpdateContentSize();
        
        // 预填充可见单元格
        int visibleCount = GetVisibleCellCount();
        for(int i = 0; i < visibleCount + 2; i++) // 额外缓冲
        {
            AddCell(i);
        }
    }

    void UpdateContentSize()
    {
        float totalSize = cellSize * dataCount + spacing * (dataCount - 1);
        if(direction == Direction.Vertical)
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalSize);
        }
        else
        {
            content.sizeDelta = new Vector2(totalSize, content.sizeDelta.y);
        }
    }

    int GetVisibleCellCount()
    {
        if(direction == Direction.Vertical)
        {
            return Mathf.CeilToInt(viewport.rect.height / (cellSize + spacing)) + 1;
        }
        return Mathf.CeilToInt(viewport.rect.width / (cellSize + spacing)) + 1;
    }

    void AddCell(int index)
    {
        RectTransform cell = GetCellFromPool();
        UpdateCellPosition(cell, index);
        UpdateCellData(cell, index);
        activeCells.Add(cell);
    }

    RectTransform GetCellFromPool()
    {
        if(cellPool.Count > 0)
        {
            RectTransform cell = cellPool.Dequeue();
            cell.gameObject.SetActive(true);
            return cell;
        }
        return Instantiate(cellPrefab, content).GetComponent<RectTransform>();
    }

    void UpdateCellPosition(RectTransform cell, int index)
    {
        float pos = index * (cellSize + spacing);
        if(direction == Direction.Vertical)
        {
            cell.anchoredPosition = new Vector2(0, -pos);
        }
        else
        {
            cell.anchoredPosition = new Vector2(pos, 0);
        }
    }

    void UpdateCellData(RectTransform cell, int index)
    {
        // 这里更新单元格显示内容
        cell.GetComponent<Cell>().Init(index);
    }

    void OnScroll(Vector2 normalizedPos)
    {
        UpdateVisibleCells();
    }

    void UpdateVisibleCells()
    {
        // 计算可见范围
        float startPos = direction == Direction.Vertical ? 
            -content.anchoredPosition.y : 
            content.anchoredPosition.x;
        
        int firstVisibleIndex = Mathf.FloorToInt((startPos - cellSize) / (cellSize + spacing));
        firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, dataCount - 1);

        if(firstVisibleIndex != currentFirstIndex)
        {
            // 回收不可见单元格
            while(activeCells.Count > 0 && 
                GetCellIndex(activeCells[0]) < firstVisibleIndex)
            {
                RecycleCell(activeCells[0]);
                activeCells.RemoveAt(0);
            }

            // 添加新单元格
            int lastIndex = GetCellIndex(activeCells[activeCells.Count - 1]);
            while(lastIndex < firstVisibleIndex + GetVisibleCellCount() && 
                lastIndex < dataCount - 1)
            {
                AddCell(lastIndex + 1);
                lastIndex++;
            }

            currentFirstIndex = firstVisibleIndex;
        }
    }

    int GetCellIndex(RectTransform cell)
    {
        float pos = direction == Direction.Vertical ? 
            -cell.anchoredPosition.y : 
            cell.anchoredPosition.x;
        return Mathf.FloorToInt(pos / (cellSize + spacing));
    }

    void RecycleCell(RectTransform cell)
    {
        cell.gameObject.SetActive(false);
        cellPool.Enqueue(cell);
    }

    // 直接跳转
    public void JumpToIndexImmediate(int index)
    {
        index = Mathf.Clamp(index, 0, dataCount - 1);
        float targetPos = index * (cellSize + spacing);
        
        if(direction == Direction.Vertical)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, targetPos);
        }
        else
        {
            content.anchoredPosition = new Vector2(-targetPos, content.anchoredPosition.y);
        }
    }

    // 动画跳转
    public void JumpToIndexSmooth(int index)
    {
        index = Mathf.Clamp(index, 0, dataCount - 1);
        float targetPos = index * (cellSize + spacing);
        
        if(direction == Direction.Vertical)
        {
            content.DOAnchorPosY(targetPos, animationDuration)
                .SetEase(animationEase);
        }
        else
        {
            content.DOAnchorPosX(-targetPos, animationDuration)
                .SetEase(animationEase);
        }
    }
}

// 单元格示例组件
public class Cell : MonoBehaviour
{
    [SerializeField] private Text indexText;

    public void Init(int index)
    {
        indexText.text = index.ToString();
    }
}