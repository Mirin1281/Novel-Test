﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

// https://qiita.com/keel/items/11ec62aff184a3a2cccc
[AttributeUsage(AttributeTargets.Field)]
class SceneChangerAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneChangerAttribute))]
public class SceneChangerEditor : PropertyDrawer
{
    List<string> SceneNameList
    {
        get
        {
            List<string> sceneNames = new ();
            // BuildSettingsからSceneのPathを読み込む
            List<string> AllPaths = (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToList();
            // PathからScene名を切り出す
            foreach (string x in AllPaths)
            {
                int slash = x.LastIndexOf("/");
                int dot = x.LastIndexOf(".");
                sceneNames.Add(x.Substring(slash + 1, dot - slash - 1));
            }
            return sceneNames;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var list = SceneNameList;
        var selectedIndex = list.FindIndex(item => item.Equals(property.stringValue));
        if (selectedIndex == -1)
        {
            selectedIndex = list.FindIndex(item => item.Equals(list[0]));
        }

        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, list.ToArray());

        property.stringValue = list[selectedIndex];
    }
}
#endif