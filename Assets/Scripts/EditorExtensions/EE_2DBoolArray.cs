using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PartSO))]
public class BoolArrayEditor : Editor
{
    SerializedProperty boolArray;
    SerializedProperty sprite;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        boolArray = serializedObject.FindProperty("shape");
        if (boolArray == null)
        {
            Debug.LogError("boolArray is null");
            return;
        }

        Rect r = Rect.zero;

        for (int i = 0; i < PartController.maxHeight; i++)
        {
            Rect rr = EditorGUILayout.BeginHorizontal();

            if(i == 0)
                r = rr;


            for (int j = 0; j < PartController.maxHeight; j++)
            {
                SerializedProperty element = boolArray.GetArrayElementAtIndex(i*PartController.maxWidth+j);
                element.boolValue = EditorGUILayout.Toggle(element.boolValue, GUILayout.Width(20));
            }

            EditorGUILayout.EndHorizontal();
        }
        
        sprite = serializedObject.FindProperty("sprite");
        Sprite s = (Sprite)sprite.objectReferenceValue;
        if (s != null)
        {
            DrawTexturePreview(Rect.MinMaxRect(r.xMin, r.yMin + 2, r.xMin + 60, r.yMin + 56),s, new Color(0.5f,0.5f,0.5f,0.33f));
        }
        serializedObject.ApplyModifiedProperties();
    }

    //https://forum.unity.com/threads/drawing-a-sprite-in-editor-window.419199/
    private void DrawTexturePreview(Rect position, Sprite sprite, Color? color = null)
    {
        Color c = color ?? Color.white;
        Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
        Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        Rect coords = sprite.textureRect;
        coords.x /= fullSize.x;
        coords.width /= fullSize.x;
        coords.y /= fullSize.y;
        coords.height /= fullSize.y;

        Vector2 ratio;
        ratio.x = position.width / size.x;
        ratio.y = position.height / size.y;
        float minRatio = Mathf.Min(ratio.x, ratio.y);

        Vector2 center = position.center;
        position.width = size.x * minRatio;
        position.height = size.y * minRatio;
        position.center = center;

        Color guic = GUI.color;
        GUI.color = c;
        GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
        GUI.color = guic;
    }
}
