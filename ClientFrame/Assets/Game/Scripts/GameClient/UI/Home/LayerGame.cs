using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LayerGame:LayerBase
{
    [SerializeField] private RectTransform nodeGame;
    [SerializeField] private RectTransform nodeLines;
    [SerializeField] private RectTransform prefabLine1;
    [SerializeField] private RectTransform prefabLine2;
    [SerializeField] private ViewGameCell prefabCell;
    
    [SerializeField] private ViewGameDragCell prefabDragCell;

    [SerializeField] public RectTransform nodeDrag;
    [SerializeField] private RectTransform nodeRandom;

    
    private CubsManager _cubsManager;
    private int _rows = 10;
    private int _cols = 12;
    private int _cellSize = 100;
    //左边界
    private float _leftPos = 0;
    //上边界
    private float _topPos = 0;
    private List<GameObject> _lines;
    
    
    private ViewGameCell[,] _cells;

   
    private int[] _cacheCheckPos = new  int[]{-1,-1};
    private List<ViewGameCell> _preCells = new List<ViewGameCell>();
    public override void OnInit(object data = null)
    {
        base.OnInit(data);
        InitData();
    }

    private void InitData()
    {
        _cubsManager = CubsManager.Instance;
        _cubsManager.NewGame(_rows, _cols);
        
        InitUI();
        _leftPos = -_rows/2f * _cellSize;
        _topPos = _cols/2f * _cellSize;
    }

 

    private void InitUI()
    {
        //盘面大小
        var sizeDelta = nodeGame.sizeDelta;
        sizeDelta = new Vector2(sizeDelta.x, sizeDelta.x * _cols / _rows);
        nodeGame.sizeDelta = sizeDelta;
        
        //格子线
        _lines = new List<GameObject>();
        prefabLine2.sizeDelta = new Vector2(prefabLine2.sizeDelta.x, sizeDelta.y);
        for(var i =0;i<_cols;i++)
        {
            var line = Instantiate(prefabLine1, nodeLines);
            line.name = $"Line row{i}";
            line.anchoredPosition = new Vector3(0, prefabLine1.anchoredPosition.y - (i + 1) * _cellSize, 0);
            _lines.Add(line.gameObject);
        }
        for (var j = 0; j < _rows; j++)
        {
            var line = Instantiate(prefabLine2, nodeLines);
            line.name = $"Line col{j}";
            line.anchoredPosition = new Vector3(prefabLine2.anchoredPosition.x + (j + 1) * _cellSize, 0, 0);
            _lines.Add(line.gameObject);
        }
        
        _cells = new ViewGameCell[_rows,_cols];
        //格子
        for(var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _cols; j++)
            {
                var cell = Instantiate(prefabCell, nodeGame);
                cell.name = $"Cell {i} {j}";
                cell.transform.localPosition = new Vector3((i - _rows/2+0.5f) * 100, (-j+_cols/2-0.5f) * 100, 0);
                var cellData = new CubeCell(i,j,_cubsManager.Data[i,j]);
                cell.Init(cellData);
                _cells[i, j] = cell;
            }
        }
        
        //随机模块
        for (var i = 0; i < 3; i++)
        {
            var dragCell = Instantiate(prefabDragCell, nodeRandom);
            dragCell.name = $"DragCell {i}";
            dragCell.transform.localPosition = new Vector3(-300+i*300,0 , 0);
            var randomData = _cubsManager.RandomData();
            dragCell.Init(randomData,this);
        }
    }



    private void ClearUI()
    {
        _lines.ForEach(Destroy);
    }


    public void OnCellMove(Vector3 pointPos,int[,] data)
    {
        
        var x = Mathf.FloorToInt((pointPos.x - _leftPos)/_cellSize);
        var y = Mathf.FloorToInt((_topPos - pointPos.y)/_cellSize);
        if(_cacheCheckPos[0] == x && _cacheCheckPos[1] == y)
        {
            return;
        }
        _cacheCheckPos[0] = x;
        _cacheCheckPos[1] = y;
        foreach (var cell in _preCells)
        {
            cell.ShowPreView(false);
        }
        _preCells.Clear();
        var checkData = LogicUtil.GetInputB2AChangeData(_cubsManager.Data, data, x, y);
        
        if (checkData != null)
        {
            
            for (var i = 0; i < checkData.Count; i++)
            {
                var pos = checkData[i];
                var cell = _cells[pos.x, pos.y];
                cell.ShowPreView(true);
                _preCells.Add(cell);
            }
        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public void OnClickTestData()
    {
        UIManager.Instance.OpenView(WindowID.ViewTestData);
    }
    
}
