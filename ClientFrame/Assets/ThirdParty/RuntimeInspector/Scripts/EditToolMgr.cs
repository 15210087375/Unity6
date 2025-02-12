using System;
using System.Collections;
using System.Collections.Generic;
using RuntimeInspectorNamespace;
using UnityEngine;
using UnityEngine.UI;

public class EditToolMgr : MonoBehaviour
{
    private static EditToolMgr _instance;
    public static EditToolMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EditToolMgr>();
            }
            return _instance;
        }
    }
    public GameObject nodeTools;
    public GameObject nodeEdit;
    public Slider alphaSlider;
    public List<GameObject> alphaCtrlList;
    
    public RuntimeHierarchy hierarchy;
    public RuntimeInspector inspector;
    
    private void Awake()
    {
        
        alphaSlider.onValueChanged.AddListener(OnValueChange);
    }

    private void Start()
    {
        nodeTools.gameObject.SetActive(false);
        nodeEdit.gameObject.SetActive(false);
        alphaSlider.value = 0.8f;
        hierarchy.SetSkin();
        inspector.SetSkin();
    }

    public void OpenTool()
    {
        nodeTools.SetActive(true);
    }

    public void CloseTool()
    {
        nodeTools.SetActive(false);
    }

    public void OpenEdit()
    {
        // nodeEdit.SetActive(!nodeEdit.activeSelf);
    }
   
    private void OnValueChange(float value)
    {
        // for (var i = 0; i < alphaCtrlList.Count; i++)
        // {
        //     var go = alphaCtrlList[i];
        //     // var imgs = go.GetComponentsInChildren<Image>();
        //     // foreach (var image in imgs)
        //     // {
        //     //     var c = image.color;
        //     //     image.color = new Color(c.r, c.g, c.b, value);
        //     // }
        //     var cg = go.GetComponent<CanvasGroup>();
        //     if (cg != null)
        //     {
        //         cg.alpha = value;
        //     }
        // }

    }

    private void Update()
    {
        
    }
    // private int _count = 0;
    // public void OnOpenRuntTimeLogClick()
    // {
    //    
    //     if (!nodeTools.activeSelf)
    //     {
    //         _count++;
    //         if (_count == 2)
    //         {
    //             nodeTools.SetActive(true);
    //         }
    //         QueueAnimation.WaitFunction(0.5f, () =>
    //         {
    //             _count = 0;
    //         });
    //     }
    //     else
    //     {
    //         _count = 0;
    //         nodeTools.SetActive(false);
    //     }
    // }
}
