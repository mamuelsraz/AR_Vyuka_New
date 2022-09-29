using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArObject))]
public class ArObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ArObject obj = (ArObject)target;

        base.OnInspectorGUI();

        GUILayout.Space(20);

        if (GUILayout.Button("Setup bundle"))
        {
            SetupBundle(obj);
        }
    }

    void SetupBundle(ArObject target)
    {
        EditorUtility.SetDirty(target);

        if (target.obj == null)
        {
            Debug.LogError("obj is null");
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(target.obj.GetInstanceID());
        target.bundle = AssetImporter.GetAtPath(assetPath).assetBundleName;

        assetPath = AssetDatabase.GetAssetPath(target.GetInstanceID());
        AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant("List", "");
    }
}
