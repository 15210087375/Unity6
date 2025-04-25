using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine.UI;

namespace ExtendUI
{
    [CustomEditor(typeof(ExtendImage), true), CanEditMultipleObjects]
    public class ExtendImageEditor : ImageEditor
    {
        private SerializedProperty m_Sprite;
        private SerializedProperty m_Type;
        private SerializedProperty m_PreserveAspect;
        private SerializedProperty m_UseSpriteMesh;

        private AnimBool           m_ShowImgType;
        private SerializedProperty m_FillMethod;
        private SerializedProperty m_SlicedClipMode;

        private SerializedProperty m_ShapeMode;
        private SerializedProperty m_ShapeOffset;

        protected override void OnEnable()
        {
            m_Sprite         = serializedObject.FindProperty("m_Sprite");
            m_Type           = serializedObject.FindProperty("m_Type");
            m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");
            m_UseSpriteMesh  = serializedObject.FindProperty("m_UseSpriteMesh");
            m_FillMethod = serializedObject.FindProperty("m_FillMethod");
            m_SlicedClipMode = serializedObject.FindProperty("slicedClipMode");
            m_ShapeMode = serializedObject.FindProperty("shapeMode");
            m_ShapeOffset = serializedObject.FindProperty("shapeOffset");
            m_ShowImgType = new AnimBool(m_Sprite.objectReferenceValue != null);
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SpriteGUI();
            AppearanceControlsGUI();
            RaycastControlsGUI();

            m_ShowImgType.target = m_Sprite.objectReferenceValue != null;
            if (EditorGUILayout.BeginFadeGroup(m_ShowImgType.faded))
                TypeGUI();
            EditorGUILayout.EndFadeGroup();

            SetShowNativeSize(false);
            if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
            {
                EditorGUI.indentLevel++;

                if ((Image.Type) m_Type.enumValueIndex == Image.Type.Simple)
                {
                    EditorGUILayout.PropertyField(m_UseSpriteMesh);
                }
                if ((Image.Type)m_Type.enumValueIndex == Image.Type.Filled)
                {
                    if ((Image.FillMethod)m_FillMethod.enumValueIndex == Image.FillMethod.Horizontal ||
                        (Image.FillMethod)m_FillMethod.enumValueIndex == Image.FillMethod.Vertical)
                        EditorGUILayout.PropertyField(m_SlicedClipMode);
                }
    
                EditorGUILayout.PropertyField(m_PreserveAspect);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
          
            if ((Image.Type)m_Type.enumValueIndex == Image.Type.Simple || 
                (Image.Type)m_Type.enumValueIndex == Image.Type.Filled)
            {
                EditorGUILayout.PropertyField(m_ShapeMode);
                if (m_ShapeMode.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(m_ShapeOffset);
                    EditorGUI.indentLevel--;
                }
            }
            
            NativeSizeButtonGUI();


            serializedObject.ApplyModifiedProperties();
        }

        private void SetShowNativeSize(bool instant)
        {
            var type           = (Image.Type) m_Type.enumValueIndex;
            var showNativeSize = (type == Image.Type.Simple || type == Image.Type.Filled) && m_Sprite.objectReferenceValue != null;
            base.SetShowNativeSize(showNativeSize, instant);
        }
    }
}

