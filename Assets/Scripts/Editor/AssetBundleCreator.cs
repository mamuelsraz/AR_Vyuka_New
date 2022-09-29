using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleCreator
{
    [MenuItem("Tools/Create/AssetBundles for Windows")]
    static void BuildAllAssetBundlesWindows()
    {
        string assetBundleDirectory = "Assets/Bundles/AR_Vyuka_New_Content";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/Create/AssetBundles for Android")]
    static void BuildAllAssetBundlesAndroid()
    {
        string assetBundleDirectory = "Assets/Bundles/AR_Vyuka_New_Content";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.Android);
    }

    [MenuItem("Tools/Create/AssetBundles for IOS")]
    static void BuildAllAssetBundlesIOS()
    {
        string assetBundleDirectory = "Assets/Bundles/AR_Vyuka_New_Content";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.iOS);
    }
}
