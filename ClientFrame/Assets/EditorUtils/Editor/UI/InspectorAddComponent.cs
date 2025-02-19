using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Inspector面板添加组件回调(优化component)
/// </summary>
[InitializeOnLoad]
public class InspectorAddComponent
{
    static InspectorAddComponent()
    {
        //监听组件添加事件
        ObjectFactory.componentWasAdded += ComponentWasAdded;
       
    }
    
    private static void ComponentWasAdded(Component com)
    {
        switch (com.GetType().ToString())
        {
            case "UnityEngine.UI.Image":
                ComponentOptimizing.OptimizingImage(com as Image);
                break;
            case "UnityEngine.UI.Text":

                ComponentOptimizing.OptimizingText(com as Text);
                break;
            case "UnityEngine.UI.Button":

                ComponentOptimizing.OptimizingButton(com as Button);
                break;
            case "UnityEngine.UI.Mask":

                ComponentOptimizing.OptimizingMask(com as Mask);
                break;
            case "TMPro.TextMeshProUGUI":
                ComponentOptimizing.OptimizingTmp(com as TextMeshProUGUI);
                break;
        }
    }
}