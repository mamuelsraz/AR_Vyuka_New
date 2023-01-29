using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

[Serializable]
public class LanguageARAsset
{
    public string asset;
    public string icon;
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

    public LanguageBlock GetBlock(string language)
    {
        if (area != "Jazyky") return null;

        LanguageBlock block = null;
        foreach (var item in nameInLanguage)
        {
            if (item.language == language)
            {
                block = item;
                break;
            }
        }

        return block;
    }
}
