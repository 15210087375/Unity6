using System.Collections;
using System.Collections.Generic;
using ExtendUI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomUI : Editor
{
    [MenuItem("GameObject/UI/CustomButton")]
    public static void CreateCustomButton()
    {
        //创建按钮
        var btn = ObjectFactory.CreateGameObject("CustomButton");
        ObjectFactory.AddComponent<RectTransform>(btn);
        var img = ObjectFactory.AddComponent<Image>(btn);
        img.raycastTarget = true;
        img.type = Image.Type.Sliced;
        img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        ObjectFactory.AddComponent<CustomButton>(btn);
        if (!UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(btn))
        {
            //非prefab编辑模式下，需要找父节点canvas
            if (Selection.activeObject != null && Selection.activeObject is GameObject)
            {
                var go = (GameObject)Selection.activeObject;
                if (go.GetComponentInParent<Canvas>(true))
                {
                    btn.transform.SetParent(go.transform);
                }
                else
                {
                    var canvas = CreateCanvas();
                    canvas.transform.SetParent(go.transform);
                    btn.transform.SetParent(canvas.transform);
                }
            }
            else
            {
                var canvas = FindFirstObjectByType<Canvas>();
                if(canvas == null)
                {
                    canvas = CreateCanvas();
                }
                btn.transform.SetParent(canvas.transform);
            }
            
        }
        else
        {
            //prefab编辑模式下，直接添加到选中的物体下
            if (Selection.activeObject != null && Selection.activeObject is GameObject)
            {
                var go = (GameObject)Selection.activeObject;
                btn.transform.SetParent(go.transform);
            }
        }
        btn.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
        btn.transform.localPosition = Vector3.zero;
        Selection.activeObject = btn;
        Expend(btn.transform.parent,true);
        //文本
        var txt = ObjectFactory.CreateGameObject("Text");
        ObjectFactory.AddComponent<RectTransform>(txt);
        ObjectFactory.AddComponent<TextMeshProUGUI>(txt);
        txt.transform.SetParent(btn.transform);
        txt.transform.localPosition = Vector3.zero;
        txt.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
        
        //tmp
        var tmp = txt.GetComponent<TextMeshProUGUI>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;
        tmp.text = "button";
    }
    /// <summary>
    /// 创建Canvas
    /// </summary>
    /// <returns>canvas</returns>
    private static Canvas CreateCanvas()
    {
        var canvas = ObjectFactory.CreateGameObject("Canvas");
        var c = ObjectFactory.AddComponent<Canvas>(canvas);
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        ObjectFactory.AddComponent<CanvasScaler>(canvas);
        ObjectFactory.AddComponent<GraphicRaycaster>(canvas);
        canvas.layer = LayerMask.NameToLayer("UI");
        return c;
    }
    private static void Expend(Transform transform,bool expand)
    {
        SceneHierarchyUtility.SetExpanded(transform.gameObject,expand);
        if(transform.parent != null)
        {
            Expend(transform.parent,expand);
        }
    }
   
    
}
