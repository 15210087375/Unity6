using System;
using System.Collections;
using cfg;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class TestPovitAnchor : MonoBehaviour
{
    [SerializeField] private RectTransform nodeRect;
    [ProgressBar(-10f,10f),OnValueChanged("Btn")]
    [SerializeField] public float x  = 0.5f;
    [ProgressBar(-10f,10f),OnValueChanged("Btn")]
    [SerializeField] public float y = 0.5f;

    private Vector2 tempOffset = new Vector3(50, -50);
    [Button]
    public void Btn()
    {
        var x1 = Mathf.CeilToInt(x * 10) / 10f;
        var y1 = Mathf.CeilToInt(y * 10) / 10f;
        var newPivot = new Vector2(x1, y1);
        var offset = GetStretchAnchorOffset(newPivot, nodeRect);
        var offset1 = GetNormalAnchorOffset(newPivot, nodeRect);
        
        var xOffset = IsXAxisStretched(nodeRect) ? offset.x : offset1.x;
        var yOffset = IsYAxisStretched(nodeRect) ? offset.y : offset1.y;
        nodeRect.pivot = newPivot;
        var newPos = new Vector2(xOffset.x + yOffset.x, xOffset.y + yOffset.y);
        StartCoroutine(SetPos(newPos));
        // nodeRect.anchoredPosition = newPos;
      

    }

    private IEnumerator SetPos(Vector2 pos)
    {
        yield return null;
        nodeRect.anchoredPosition = pos+tempOffset;
    }
 
    
   
    private (Vector2 x,Vector2 y) GetStretchAnchorOffset(Vector2 newPivot,RectTransform rect)
    {
        var size = rect.rect.size;
        var offsetTempX = 0.5f * size.x; 
        var ro = rect.eulerAngles.z*Mathf.Deg2Rad;
        var line1 = offsetTempX * Mathf.Sin(ro);
        var line2 = offsetTempX * Mathf.Cos(ro);
        var line3 = offsetTempX - line2;
        var rate =(newPivot.x - 0.5f)/0.5f;
        var offsetY = rate * line1;
        var offsetX = rate * line3;
        
    
        var offsetTempY = 0.5f * size.y; 
        var line11 = offsetTempY * Mathf.Sin(ro);
        var line22 = offsetTempY * Mathf.Cos(ro);
        var line33 = offsetTempY - line22;
        var rate1 =(newPivot.y - 0.5f)/0.5f;
        var offsetX1 = rate1 * line11;
        var offsetY1 = rate1 * line33;
     
        var xTemp = new Vector2(-offsetX,+offsetY);
        var yTemp = new Vector2(-offsetX1,-offsetY1);
        return (xTemp,yTemp);
        // rect.anchoredPosition = new Vector2(-offsetX1-offsetX,-offsetY1+offsetY);
    }
    
    private bool IsXAxisStretched(RectTransform rectTransform)
    {
        return Math.Abs(rectTransform.anchorMin.x - rectTransform.anchorMax.x) > 0.01f;
    }
    
    private bool IsYAxisStretched(RectTransform rectTransform)
    {
        return Math.Abs(rectTransform.anchorMin.y - rectTransform.anchorMax.y) > 0.01f;
    }
    
   
    [Button]
    public void Reset()
    {
        x = 0.5f;
        y = 0.5f;
        nodeRect.pivot = new Vector2(0.5f, 0.5f);
        StartCoroutine(SetPos(Vector2.zero));
    }
  
    private (Vector2 x,Vector2 y) GetNormalAnchorOffset(Vector2 newPivot,RectTransform rect)
    {
        var size = rect.rect.size;
        var offsetTempX = 0.5f * size.x; 
        var ro = rect.eulerAngles.z*Mathf.Deg2Rad;
        var line1 = offsetTempX * Mathf.Cos(ro);   
        var line2 = offsetTempX * Mathf.Sin(ro);
        var rate = (newPivot.x - 0.5f)/0.5f;
        var xOffset = line1 * rate;
        var yOffset = line2 * rate;
        
        var offsetTempY = 0.5f * size.y; 
        var line11 = offsetTempY * Mathf.Sin(ro);
        var line22 = offsetTempY * Mathf.Cos(ro);
        var rate1 = (newPivot.y - 0.5f)/0.5f;
        var xOffset1 = line11 * rate1;
        var yOffset1 = line22 * rate1;
      
   
        var tempX = new Vector2( xOffset, yOffset);
        var tempY = new Vector2(-xOffset1, yOffset1);
        return (tempX, tempY);
    }
    
}
