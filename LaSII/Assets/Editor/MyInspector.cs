using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MyScript : MonoBehaviour
{
    public bool flag;
    public int i = 1;
}
public class MyInspector : Editor
{
    [CustomEditor(typeof(MyScript))]

    void OnInspectorGUI()
    {
        var myScript = target as MyScript;

        myScript.flag = GUILayout.Toggle(myScript.flag, "Flag");

        if (myScript.flag)
            myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);

    }
}
