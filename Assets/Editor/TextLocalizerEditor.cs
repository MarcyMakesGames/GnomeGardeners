using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TextLocalizerEditWindow : EditorWindow
{
    public static void Open(string key)
    {
        TextLocalizerEditWindow window = CreateInstance<TextLocalizerEditWindow>();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();
        window.key = key;
    }

    public string key;
    public string value;

    public void OnGUI()
    {
        key = EditorGUILayout.TextField("Key:", "");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50f));

        EditorStyles.textArea.wordWrap = true;
        value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100f), GUILayout.Width(400f));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            if(LocalizationManager.GetLocalizedValue(key) != string.Empty)
            {
                LocalizationManager.Replace(key, value);
            }
            else
            {
                LocalizationManager.Add(key, value);
            }
        }

        minSize = new Vector2(460f, 150f);
        maxSize = minSize;
    }
}

public class TextLocalizerSearchWindow : EditorWindow
{
    public static void Open()
    {
        TextLocalizerSearchWindow window = CreateInstance<TextLocalizerSearchWindow>();
        window.titleContent = new GUIContent("Localization Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect r = new Rect(mouse.x - 450f, mouse.y + 10f, 10f, 10f);
        window.ShowAsDropDown(r, new Vector2(500f, 300f));
    }

    public string value;
    public Vector2 scroll;
    public Dictionary<string, string> dictionary;

    private void OnEnable()
    {
        dictionary = LocalizationManager.GetDictionaryForEditor();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();

        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if(value == null) { return; }

        EditorGUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach(KeyValuePair<string, string> element in dictionary)
        {
            if(element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("box");
                Texture closeIcon = (Texture)Resources.Load("close");

                GUIContent content = new GUIContent(closeIcon);

                if(GUILayout.Button(content, GUILayout.MaxWidth(20f), GUILayout.MaxHeight(20f)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localization, are you sure?", "Do it"))
                    {
                        LocalizationManager.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationManager.Init();
                        dictionary = LocalizationManager.GetDictionaryForEditor();
                    }
                }

                EditorGUILayout.TextField(element.Key);
                EditorGUILayout.LabelField(element.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}