
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewGameCell:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDesc;
    [SerializeField] private Image imgNormal;
    private CubeCell data;

    
    public void Init(CubeCell cellData)
    {
        textDesc.text = $"{cellData.X},{cellData.Y}";
        Refresh(cellData);
    }

    public void HidePreView()
    {
        imgNormal.gameObject.SetActive(false);
    }
    public void ShowPreView(CubeCell cellData)
    {
        Debug.Log("ShowPreView");
        ShowUI(cellData.cellType, cellData.colorType,true);
    }

    public void Refresh(CubeCell cellData)
    {
        data = cellData;
        ShowUI(cellData.cellType, cellData.colorType);
    }



    private void ShowUI(CellType type,ColorType color,bool isPreView = false)
    {
        if (type == CellType.Normal)
        {
            imgNormal.gameObject.SetActive(true);

            imgNormal.color = color switch
            {
                ColorType.Blue => Color.blue,
                ColorType.Green => Color.green,
                ColorType.Red => Color.red,
                ColorType.Yellow => Color.yellow,
                _ => throw new ArgumentOutOfRangeException()
            };
            imgNormal.SetAlpha(isPreView ? 0.5f : 1f);
        }
        else
        {
            imgNormal.gameObject.SetActive(false);
        }
    }
    
}