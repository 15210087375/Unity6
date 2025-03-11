using System.Collections.Generic;
using UnityEngine;

public class LayerGame:LayerBase
{
    [SerializeField] private RectTransform nodeGame;
    [SerializeField] private RectTransform nodeLines;
    [SerializeField] private RectTransform prefabLine1;
    [SerializeField] private RectTransform prefabLine2;
    [SerializeField] private ViewGameCell prefabCell;

    [SerializeField] private RectTransform nodeDrag;

    
    private CubsManager _cubsManager;
    private int _rows = 10;
    private int _cols = 12;

    private List<GameObject> _lines;
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
            line.anchoredPosition = new Vector3(0, prefabLine1.anchoredPosition.y - (i + 1) * 100, 0);
            _lines.Add(line.gameObject);
        }
        for (var j = 0; j < _rows; j++)
        {
            var line = Instantiate(prefabLine2, nodeLines);
            line.name = $"Line col{j}";
            line.anchoredPosition = new Vector3(prefabLine2.anchoredPosition.x + (j + 1) * 100, 0, 0);
            _lines.Add(line.gameObject);
        }
        
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
            }
        }
    }



    private void ClearUI()
    {
        _lines.ForEach(Destroy);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public void OnClickTestData()
    {
        UIManager.Instance.OpenView(WindowID.ViewTestData);
    }
    
}
