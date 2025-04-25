using ExtendUI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 组件优化设置
/// </summary>
public class ComponentOptimizing
{
    #region Image

    public static void OptimizingImage(Image image)
    {
        //如果当前图片不是按钮，取消勾选RaycastTarget
        if (image.gameObject.GetComponent<Button>() == null)
        {
            image.raycastTarget = false;
        }
    }

    #endregion

    #region TextMeshProUGUI
    private static string FontResPath = "Assets/Res/Font/AlibabaPuHuiTi-3500-Heavy SDF.asset"; //字体资源路径
    public static void OptimizingTmp(TextMeshProUGUI tmp)
    {
        if (tmp.gameObject.GetComponent<MaskableGraphic>() == null )
        {
            tmp.raycastTarget = false;
        }
        tmp.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontResPath);
        tmp.alignment = TextAlignmentOptions.Center;
       
    }

    #endregion
    #region Text

    public static void OptimizingText(Text text)
    {
        if (text.gameObject.GetComponent<MaskableGraphic>() == null)
        {
            text.raycastTarget = false;
        }

        //是否支持富文字框架，默认不支持，有需求再手动勾选
        text.supportRichText = false;
        
       
    }

    #endregion

    #region Button

    public static void OptimizingButton(Button button)
    {
        //判断需要添加button组件的物体是否有继承自MaskableGraphic的组件，有的话就勾选RaycastTarget
        if (button.gameObject.GetComponent<MaskableGraphic>() != null)
        {
            button.gameObject.GetComponent<MaskableGraphic>().raycastTarget = true;
        }

        if(button.gameObject.GetComponent<CustomButton>() == null)
        {
            var go = button.gameObject;
            // 将删除操作延迟到下一个 Unity 帧
            EditorApplication.delayCall += () => {
                Object.DestroyImmediate(go.GetComponent<Button>());
                go.AddComponent<CustomButton>();
            };
        }
    }

    private static void AddListener(Button button, UnityAction e)
    {
        // 获取或创建自定义序列化对象和属性
        SerializedObject serializedButton = new SerializedObject(button);
        // 将新创建的 UnityEvent 添加到 Button 的 OnClick 事件中
        UnityEventTools.AddVoidPersistentListener(button.onClick, e);
        // 应用所有更改并重新绘制 Inspector 视图
        serializedButton.ApplyModifiedProperties();
    }

    #endregion

    #region Mask

    public static void OptimizingMask(Mask mask)
    {
        //判断需要添加mask组件的物体是否有继承自MaskableGraphic的组件，有的话就勾选RaycastTarget
        if (mask.gameObject.GetComponent<MaskableGraphic>() != null)
        {
            mask.gameObject.GetComponent<MaskableGraphic>().raycastTarget = true;
        }
    }

    #endregion
}