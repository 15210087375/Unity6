
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : UnityEditor.UI.ButtonEditor
{
    private SerializedProperty _onLongPressProperty;
    private SerializedProperty _onPressBeginProperty;
    private SerializedProperty _onPressProperty;
    private SerializedProperty _onPressEndProperty;
    private SerializedProperty _longPressTimeProperty;
    private SerializedProperty _doClickAndLongPressProperty;
    private SerializedProperty _soundIdProperty;
    
    private SerializedProperty _customAnimType;
    private SerializedProperty _customAnimObject;
    private SerializedProperty _customEase;
    private SerializedProperty _customAnimScaleDuration;
    private SerializedProperty _customAnimScaleValue;
    private SerializedProperty _customAnimScaleValueNormal;
    
    private bool _showPress = false;
    private bool _showCustomAnim = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        _onLongPressProperty = serializedObject.FindProperty("onLongPress");
        _onPressBeginProperty = serializedObject.FindProperty("onPressBegin");
        _onPressProperty = serializedObject.FindProperty("onPress");
        _onPressEndProperty = serializedObject.FindProperty("onPressEnd");
        _longPressTimeProperty = serializedObject.FindProperty("longPressTime");
        _soundIdProperty = serializedObject.FindProperty("soundId");
        _doClickAndLongPressProperty = serializedObject.FindProperty("doClickAndLongPress");
        
        _customAnimType = serializedObject.FindProperty("customAnimType");
        _customAnimObject = serializedObject.FindProperty("animObject");
        _customEase = serializedObject.FindProperty("ease");
        _customAnimScaleDuration = serializedObject.FindProperty("scaleDuration");
        _customAnimScaleValue = serializedObject.FindProperty("scaleValue");
        _customAnimScaleValueNormal = serializedObject.FindProperty("scaleValueNormal");
        
        _showPress = _onLongPressProperty.boxedValue != null &&
                     ((Button.ButtonClickedEvent)_onLongPressProperty.boxedValue).GetPersistentEventCount() > 0 || 
                     _onPressBeginProperty.boxedValue != null &&
                     ((Button.ButtonClickedEvent)_onPressBeginProperty.boxedValue).GetPersistentEventCount() > 0 ||
                     _onPressProperty.boxedValue != null && 
                     ((Button.ButtonClickedEvent)_onPressProperty.boxedValue).GetPersistentEventCount() > 0  ||
                     _onPressEndProperty.boxedValue != null && 
                     ((Button.ButtonClickedEvent)_onPressEndProperty.boxedValue).GetPersistentEventCount() > 0 ;
       
     
    }

    public override void OnInspectorGUI()
    {
        // var btn = (CustomButton)target;
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        _soundIdProperty.intValue = EditorGUILayout.IntField("音效ID", _soundIdProperty.intValue);
        if(GUILayout.Button("试听"))
        {
            var btn = (CustomButton)target;
            btn.OnPlaySound();
        }
        EditorGUILayout.EndHorizontal();
        // 显示折叠块标题
        _showPress = EditorGUILayout.BeginFoldoutHeaderGroup(_showPress, "Press Event");

        if (_showPress)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_onLongPressProperty);
            _longPressTimeProperty.floatValue = EditorGUILayout.FloatField("按下多久算长按", _longPressTimeProperty.floatValue);
            _doClickAndLongPressProperty.boolValue =EditorGUILayout.Toggle("同时触发点击和长按", _doClickAndLongPressProperty.boolValue);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_onPressBeginProperty);
            EditorGUILayout.PropertyField(_onPressProperty);
            EditorGUILayout.PropertyField(_onPressEndProperty);
        }

        // 结束折叠块
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space();
        //动画
        EditorGUILayout.PropertyField(_customAnimType, new GUIContent("动画类型"));
       
        _showCustomAnim = _customAnimType.enumValueIndex != 0;
        if (_showCustomAnim)
        {
            
            EditorGUILayout.PropertyField(_customAnimObject, new GUIContent("动画对象"));
            if(_customAnimType.enumValueIndex == 1)
            {
                EditorGUILayout.PropertyField(_customAnimScaleDuration, new GUIContent("动画时间"));
                EditorGUILayout.PropertyField(_customAnimScaleValue, new GUIContent("动画缩放值"));
                EditorGUILayout.PropertyField(_customAnimScaleValueNormal, new GUIContent("动画缩放值(正常)"));
                EditorGUILayout.PropertyField(_customEase, new GUIContent("动画曲线"));
            }
        }
       
      
        serializedObject.ApplyModifiedProperties();
    }
}
