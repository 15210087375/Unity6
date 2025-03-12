
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewGameDragCell:MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{

    [SerializeField] private ViewGameCell cellPrefab;
    [SerializeField] private RectTransform nodeCell;
    
    private LayerGame _layerGame;
    
    private int[,] _data;

    private Vector3 _dragStartPos;
    private Vector3 _dragOffset;
    public void Init(int[,] cellData,LayerGame layer)
    {
        _data = cellData;
        _layerGame = layer;
        var row = _data.GetLength(0);
        var col = _data.GetLength(1);
        nodeCell.sizeDelta = new Vector2(row* 75, col * 75);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                var cell = Instantiate(cellPrefab, nodeCell);
                var data = new CubeCell(i, j, _data[i, j]);
                cell.GetComponent<RectTransform>().sizeDelta *= 0.75f;
                cell.Init(data);
                cell.GetComponent<RectTransform>().localPosition =
                    new Vector2((i - row / 2f + 0.5f) * 75, (-j + col / 2f -0.5f ) * 75);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPos = eventData.position;
        _dragOffset = _dragStartPos - transform.position; //transform.InverseTransformPoint(eventData.position);
        nodeCell.transform.SetParent(_layerGame.nodeDrag);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        nodeCell.position = eventData.position - new Vector2(_dragOffset.x, _dragOffset.y);
        CheckTopLeftPoint();
    }
    
    private void CheckTopLeftPoint()
    {
        var localPosition = nodeCell.localPosition;
        var sizeDelta = nodeCell.sizeDelta;
        var pointPos = localPosition + new Vector3( -sizeDelta.x/2, sizeDelta.y/2, 0);
        _layerGame.OnCellMove(pointPos,_data);
    }
}