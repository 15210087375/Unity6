using System.Collections.Generic;
using System.Linq;
using cfg.Shop;
using UnityEngine;

public class LayerShop:LayerBase
{
    [SerializeField] private ViewShopCell viewShopCell;
    [SerializeField] private RectTransform nodeContent;
    
    private List<StoreRecord> _storeRecords;
    private List<ViewShopCell> _viewShopCells;
    private void Awake()
    {
        // Initialize the store records
        InitCells();
    }

    private void InitCells()
    {
        _storeRecords = TableManager.Instance.Tables.Store.DataList;
        _viewShopCells = nodeContent.GetComponentsInChildren<ViewShopCell>().ToList();
        var topOffset = 300;
        var bottomOffset = 200;
        nodeContent.sizeDelta = new Vector2(nodeContent.sizeDelta.x, _storeRecords.Count/2 * 550+topOffset+bottomOffset);
        for (var i = 0; i < _storeRecords.Count; i++)
        {
            var cell = i<_viewShopCells.Count ? _viewShopCells[i] : Instantiate(viewShopCell, nodeContent);
            var storeRecord = _storeRecords[i];
            cell.Init(storeRecord);
            cell.gameObject.SetActive(true);
            cell.GetComponent<RectTransform>().anchoredPosition = new Vector3(-250+i%2*500, -topOffset-i/2 * 550, 0);
        }
   
    }
}
