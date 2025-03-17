using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LayerGame:LayerBase
{
    [SerializeField] private RectTransform nodeGame;
    [SerializeField] private RectTransform nodeLines;
    [SerializeField] private RectTransform prefabLine1;
    [SerializeField] private RectTransform prefabLine2;
    [SerializeField] private ViewGameCell prefabCell;
    
    [SerializeField] private ViewGameDragCell prefabDragCell;

    [SerializeField] private RectTransform nodeDrag;
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
    private List<ViewGameDragCell> _dragCells;

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
            line.anchoredPosition = new Vector3(0, prefabLine1.anchoredPosition.y - (i + 1) * _cellSize, 0);
            _lines.Add(line.gameObject);
        }
        for (var j = 0; j < _rows; j++)
        {
            var line = Instantiate(prefabLine2, nodeLines);
            line.anchoredPosition = new Vector3(prefabLine2.anchoredPosition.x + (j + 1) * _cellSize, 0, 0);
            _lines.Add(line.gameObject);
        }
        
        
        //格子
        _cells = new ViewGameCell[_rows,_cols];
        for(var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _cols; j++)
            {
                var cell = Instantiate(prefabCell, nodeGame);
                cell.name = $"Cell {i} {j}";
                cell.transform.localPosition = new Vector3((i - _rows/2+0.5f) * 100, (-j+_cols/2-0.5f) * 100, 0);
                cell.Init(_cubsManager.Cells[i,j]);
                _cells[i, j] = cell;
            }
        }
        
        InitDragCells();
    }

    private void InitDragCells()
    {
        //拖拽的图形
        _dragCells = new List<ViewGameDragCell>();
        for (var i = 0; i < _cubsManager.DragGroups.Count; i++)
        {
            var dragCell = Instantiate(prefabDragCell, nodeRandom);
            _dragCells.Add(dragCell);
            dragCell.transform.localPosition = new Vector3(-300+i*300,0 , 0);
            dragCell.Init(_cubsManager.DragGroups[i],this);
        }
    }
    [Button("ResetGame")]
    public void ResetGame()
    {
        ClearUI();
        _cubsManager.NewGame(_rows, _cols);
        InitUI();
    }

    
    private void ResetDragCells(bool isReset = false)
    {
        if (isReset || _dragCells.Count == 0)
        {
            _cubsManager.GeneratorDragCells();
            InitDragCells();
        }
    }
    private void ClearUI()
    {
        _lines.ForEach(Destroy);
        var row = _cells.GetLength(0);
        var col = _cells.GetLength(1);
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                Destroy(_cells[i, j].gameObject);
            }
        }
        
        ClearDragCells();
    }

    private void ClearDragCells()
    {
        foreach (var cell in _dragCells)
        {
            Destroy(cell.gameObject);
        }
        _dragCells.Clear();
    }
    public void OnCellMoveStart(ViewGameDragCell cell)
    {
        cell.nodeCell.transform.SetParent(nodeDrag);
    }
    public void OnCellMove(Vector3 pointPos,CubeCellGroup cellGroup)
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
            cell.HidePreView();
        }
        _preCells.Clear();
        var checkData = _cubsManager.GetInputGroupChangeData(cellGroup, x, y);
        
        if (checkData != null)
        {
            Debug.Log($"CheckData {x}  {y}");
            for (var i = 0; i < checkData.Count; i++)
            {
                var cellData = checkData[i];
                var cell = _cells[cellData.X+x, cellData.Y+y];
                cell.ShowPreView(cellData);
                _preCells.Add(cell);
            }
        }
        
    }

    public void OnCellMoveEnd(ViewGameDragCell cell)
    {
        if(_preCells.Count == 0)
        {
            cell.MoveBack();
        }
        else
        {
            _preCells.Clear();
            cell.SetConsumed();
            _cubsManager.InputGroup(cell.GetData(), _cacheCheckPos[0], _cacheCheckPos[1]);
            RefreshCell();
            _dragCells.Remove(cell);
            Destroy(cell.gameObject);
            ResetDragCells();
            CheckMerge(cell);
            
            CheckOver();
        }
    }

    private void CheckMerge(ViewGameDragCell cell)
    {
        var group = cell._cellGroup;
        var row = group.cells.GetLength(0);
        var col = group.cells.GetLength(1);
        var removeList = _cubsManager.ClearFullLine(_cacheCheckPos[0], _cacheCheckPos[1],row,col);
        for (var i = 0; i < removeList.Count; i++)
        {
            var cellData = removeList[i];
            _cells[cellData.X, cellData.Y].Refresh(_cubsManager.Cells[cellData.X, cellData.Y]);
        }
    }
    [Button("TestCheckOver")]
    public void TestCheckOver()
    {
        CheckOver();
    }
    private void CheckOver()
    {
        var overData = _cubsManager.CheckGameOver();
        if(overData.over)
        {
            Debug.Log("GameOver");
        }
        else
        {
            Debug.Log($"CheckOver ({overData.x},{overData.y}) ");
        }
       
    }
    private void RefreshCell()
    {
        for(var i = _cacheCheckPos[0];i<_rows;i++)
        {
            for (var j = _cacheCheckPos[1]; j < _cols; j++)
            {
                var cell = _cells[i, j];
                cell.Refresh(_cubsManager.Cells[i, j]);
            }
        }
    }


    private void Update()
    {
        if (true)
        {
            
        }
    }


    public void OnClickTestData()
    {
        UIManager.Instance.OpenView(WindowID.ViewTestData);
    }
    
}
