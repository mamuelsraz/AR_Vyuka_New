using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LanguageARAsset
{
    public string asset;
    public string nickName;
    public string area;
    public string category;
    public bool hidden = false;

    public LanguageBlock[] nameInLanguage;

    public LanguageARAsset(string asset, string nickName, string area, string category, LanguageBlock[] nameInLanguage)
    {
        this.asset = asset;
        this.nickName = nickName;
        this.area = area;
        this.category = category;
        this.nameInLanguage = nameInLanguage;
    }

    public LanguageARAsset(string asset)
    {
        this.asset = asset;
    }
}
