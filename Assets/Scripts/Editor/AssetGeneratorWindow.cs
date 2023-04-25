using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets;

public class AssetGeneratorWindow : EditorWindow
{
    GameObject prefab;
    Sprite sprite;
    string label;
    AddressableAssetSettings settings;
    AddressableAssetGroup ARObjectGroup;
    AddressableAssetGroup SpriteGroup;

    [MenuItem("Tools/New ARObject")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AssetGeneratorWindow));
    }

    private void OnEnable()
    {
        this.minSize = new Vector2(400, 200);
        ARObjectGroup = AddressableAssetSettingsDefaultObject.Settings.FindGroup("ARObjects");
        SpriteGroup = AddressableAssetSettingsDefaultObject.Settings.FindGroup("Icons");
        settings = AddressableAssetSettingsDefaultObject.Settings;
    }

    public void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginVertical();

        GUILayout.Label("Create a new custom asset", EditorStyles.largeLabel);
        GUILayout.Space(20);

        prefab = EditorGUILayout.ObjectField("Prefab:", prefab, typeof(GameObject), false) as GameObject;
        GUILayout.Label("Make sure the Prefab is scaled and positioned properly", EditorStyles.helpBox);
        GUILayout.Space(10);

        sprite = EditorGUILayout.ObjectField("Sprite:", sprite, typeof(Sprite), false) as Sprite;
        label = EditorGUILayout.TextField("ID:", label);

        if (GUILayout.Button("Generate"))
        {
            GenerateAsset();
        }

        GUILayout.EndVertical();
        GUILayout.Space(10);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    void GenerateAsset() {
        if (prefab == null) {
            Debug.LogError("Prefab wasn't set");
            return;
        }
        if (sprite == null)
        {
            Debug.LogError("Sprite wasn't set");
            return;
        }
        if (label == "")
        {
            Debug.LogError("Label wasn't set");
            return;
        }

        // Log a message to the console to confirm that the asset was created and moved to the "Assets" group
        var prefabPath = AssetDatabase.GetAssetPath(prefab);
        var spritePath = AssetDatabase.GetAssetPath(sprite);

        if (!CheckAvailability(prefabPath, ARObjectGroup) || !CheckAvailability(spritePath, SpriteGroup))
            return;

        var prefabEntry = AddAssetToGroup(prefabPath, label, ARObjectGroup);
        var spriteEntry = AddAssetToGroup(spritePath, "t_" + label, SpriteGroup);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, prefabEntry, true);
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, spriteEntry, true);
        AssetDatabase.SaveAssets();

        Debug.Log("Done!");
        Debug.LogWarning("Don't forget to build all the new ARObjects in the Window/Asset Management/Addressables/Group by pressing the Build and Release button.");
        Debug.LogWarning("Also please update the reference catalogue by pressing Tools/Update JSON catalogue.");
    }

    AddressableAssetEntry AddAssetToGroup(string path, string label, AddressableAssetGroup group) {
        var guid = AssetDatabase.AssetPathToGUID(path);
        var entry = settings.CreateOrMoveEntry(guid, group);
        entry.address = label;
        return entry;
    }

    bool CheckAvailability(string path, AddressableAssetGroup group) {
        foreach (var item in group.entries)
        {
            if (item.address == label)
            {
                Debug.LogError("This label is already used!");
                return false;
            }

            if(item.guid == AssetDatabase.AssetPathToGUID(path))
            {
                Debug.LogError($"The asset {path} is already in the group!");
                return false;
            }
        }

        return true;
    }
}
