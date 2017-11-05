using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Story))]
public class StoryEditor : Editor
{
    [MenuItem("Assets/Create/Story Asset")]
    static void CreateStoryAsset()
    {
        Story story = ScriptableObject.CreateInstance<Story>();
        AssetDatabase.CreateAsset(story, "Assets/Resources/Story/DefaultStory.asset");
        AssetDatabase.Refresh();
    }

    Story story;

    private void OnEnable()
    {
        story = target as Story;
    }

    public override void OnInspectorGUI()
    {
        for (var i = 0; i < story.items.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                if (GUILayout.Button("[" + i + "]"))
                {
                    ArrayUtility.RemoveAt<Story.StoryItem>(ref story.items, i);
                    return;
                }
                story.items[i].position = EditorGUILayout.Vector2Field("Position", story.items[i].position);
                story.items[i].aniDuration = EditorGUILayout.FloatField("Duration", story.items[i].aniDuration);
                story.items[i].sprite = EditorGUILayout.ObjectField("Sprite", story.items[i].sprite, typeof(Sprite), false) as Sprite;
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("AddItem"))
            {
                ArrayUtility.Add<Story.StoryItem>(ref story.items, new Story.StoryItem());
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(story);
            AssetDatabase.SaveAssets();
        }
    }
}
