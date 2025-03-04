using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ScrollViewExpand1 : MonoBehaviour
{
    public RectTransform content;
    
    private ScrollRect _scrollRect;
    private int _dataCount;
    
    private Vector2 _scSize;
    private Vector2 _cellSize;
    private int _curSelectIndex;
    private int _leftCount;
    
    private List<ScrollViewExpandCell> _cells = new List<ScrollViewExpandCell>();
    private List<RectTransform> _cellRects = new List<RectTransform>();
    private GameObject _itemPrefab;
    private float _lastValue = -1f;
    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(OnScroll);
     
       
    }

    public void Init<T>(int count,GameObject itemPrefab,Action<T> action,int startIndex = 0)where T:ScrollViewExpandCell
    {
        _dataCount = count;
        _itemPrefab = itemPrefab;
        CheckShowCount();
        InitCells<T>(action,startIndex);
    }
   

    private void InitCells<T>(Action<T> action,int startIndex) where T:ScrollViewExpandCell
    {
        _cells.Clear();
        for(var i = 0; i < _dataCount; i++)
        {
            var cellGo = Instantiate(_itemPrefab, content.transform);
            var cell = cellGo.GetComponent<T>();
            cell.Init(this,i+_leftCount, i);
            action?.Invoke(cell);
            cellGo.SetActive(true);
            cell.transform.localPosition = new Vector3((i + _leftCount +0.5f) * _cellSize.x,0, 0);
            _cells.Add(cell.GetComponent<ScrollViewExpandCell>());
            _cellRects.Add(cell.GetComponent<RectTransform>());
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2((_dataCount + _leftCount*2) * _cellSize.x, _cellSize.y);
        _curSelectIndex = _leftCount;
        MoveToIndex(startIndex,false);
    }
    private void CheckShowCount()
    {
        _scSize = _scrollRect.GetComponent<RectTransform>().sizeDelta;
        _cellSize = _itemPrefab.GetComponent<RectTransform>().sizeDelta;
        _leftCount = Mathf.FloorToInt(_scSize.x/2 / _cellSize.x);
    }

    private void OnScroll(Vector2 value)
    {
        if (_lastValue <= -1)
        {
            _lastValue = value.x;
        }
        else
        {
            if (value.x > _lastValue)
            {
                
            }
            var tempRate = value.x > _lastValue?-0.5f:0.5f;
            var offsetX = _cellSize.x*tempRate;
            OnSelectIndex(Mathf.FloorToInt( -(content.anchoredPosition.x + offsetX)/ _cellSize.x));
        }
        
        
        var middlePos = Mathf.Abs(content.anchoredPosition.x) + _scSize.x/2;
        for (var i = 0; i < _cells.Count; i++)
        {
            var cell = _cells[i];
            var cellPosX = (i + _leftCount +0.5f) * _cellSize.x;
            var offset = middlePos - cellPosX;
            var rate = offset / _scSize.x;
            //缩放
            var scale = 1.3f - Mathf.Abs(rate / 2);
            cell.transform.localScale = Vector3.one*scale;
            //上下偏移
            var posOffsetY = -_cellSize.y*0.5f*(1-scale);
            //左右偏移
            var posOffsetX = (cell.dataIndex - _curSelectIndex) * 80f;
            var newPos = EaseManager.Evaluate(Ease.InOutQuad, null, rate, 1f, 0, 0);
            var scaledPos = 0.5f + (newPos - 0.5f) * 0.8f;
            posOffsetX = scaledPos*posOffsetX;
            cell.transform.localPosition = new Vector3(cellPosX-posOffsetX,posOffsetY,0);
        }

       
       
    }
    [Button("Move")]
    public void TestMove()
    {
        MoveToIndex(0);
    }
    public void MoveToIndex(int index,bool withAnim = true)
    {
        var offset = _cellSize.x * index;
        var targetPos = new Vector3(-offset, 0, 0);
        if (withAnim)
        {
            content.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            content.anchoredPosition = targetPos;
        }
    
        OnSelectIndex(index);
    }

    private void OnSelectIndex(int index)
    {
        if(index == _curSelectIndex)return;
        if(index < 0 || index >= _dataCount)return;
        _curSelectIndex = index;
        Debug.Log("SelectIndex:" + _curSelectIndex);
        for (var i = 0; i < _cells.Count; i++)
        {
            _cells[i].OnSelect(_curSelectIndex);
        }
        ResetShow();
    }

    private void ResetShow()
    {
        for (var i = 0; i <_cells.Count; i++)
        {
            var cell = _cells[i];
            var show = cell.dataIndex>=_curSelectIndex-_leftCount-1 && cell.dataIndex <= _curSelectIndex+_leftCount+1;
            _cells[i].gameObject.SetActive(show);
            SetSibling(cell,_cells.Count);
        }
        
        
    }

    private void SetSibling(ScrollViewExpandCell cell,int maxCount)
    {
        var index = cell.dataIndex;
        var idx = (index - _curSelectIndex)*2;
        idx = index>_curSelectIndex? Math.Abs(idx) -1:idx;
        cell.transform.SetSiblingIndex(maxCount-idx);
    }
}

