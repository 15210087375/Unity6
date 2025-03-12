
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewGameCell:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDesc;
    [SerializeField] private List<Image> imgs;
    private CubeCell data;

    
    public void Init(CubeCell cellData)
    {
        data = cellData;
        textDesc.text = data.Value.ToString();
        for (var i = 0; i < imgs.Count; i++)
        {
            var img = imgs[i];
            img.gameObject.SetActive(i == data.Value);
        }
    }

    public void ShowPreView(bool show)
    {
        imgs[2].gameObject.SetActive(show);
    }
}