using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mao))]
public class MaoEditor : Editor
{
    Mao mao;

    private void OnEnable()
    {
        mao = target as Mao;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply settings"))
        {
            mao.Init();
        }
    }
}
