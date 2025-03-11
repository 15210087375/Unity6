
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewGameDragCell:MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{

    private int[,] _data;
    
    public void Init(int[,] cellData)
    {
        _data = cellData;
        GetComponent<RectTransform>().sizeDelta = new Vector2(_data.GetLength(0) * 100, _data.GetLength(1) * 100);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}