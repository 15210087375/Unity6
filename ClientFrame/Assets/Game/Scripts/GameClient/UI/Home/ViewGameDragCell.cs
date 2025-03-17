
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewGameDragCell:MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{

    [SerializeField] private ViewGameCell cellPrefab;
    [SerializeField] public RectTransform nodeCell;
    
    private LayerGame _layerGame;

    public CubeCellGroup _cellGroup;
    private CubeCell[,] _data;

    private Vector3 _dragStartPos;
    private Vector3 _dragOffset;
    public void Init(CubeCellGroup data,LayerGame layer)
    {
        _cellGroup = data;
        _data = _cellGroup.cells;
        _layerGame = layer;
        var row = _data.GetLength(0);
        var col = _data.GetLength(1);
        nodeCell.sizeDelta = new Vector2(row* 75, col * 75);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                var cell = Instantiate(cellPrefab, nodeCell);
                var cellData = _data[i, j];
                cell.GetComponent<RectTransform>().sizeDelta *= 0.75f;
                cell.Init(cellData);
                cell.GetComponent<RectTransform>().localPosition =
                    new Vector2((i - row / 2f + 0.5f) * 75, (-j + col / 2f -0.5f ) * 75);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPos = eventData.position;
        _dragOffset = _dragStartPos - transform.position; //transform.InverseTransformPoint(eventData.position);
        _layerGame.OnCellMoveStart(this);
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
        _layerGame.OnCellMove(pointPos,_cellGroup);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _layerGame.OnCellMoveEnd(this);
    }

    public CubeCellGroup GetData()
    {
        return _cellGroup;
    }
    public void MoveBack()
    {
        nodeCell.transform.SetParent(transform);
        nodeCell.transform.DOLocalMove(Vector3.zero, 0.1f);
    }
    
    public void SetConsumed()
    {
        nodeCell.transform.SetParent(transform);
        nodeCell.gameObject.SetActive(false);
        nodeCell.transform.localPosition = Vector3.zero;
    }
}