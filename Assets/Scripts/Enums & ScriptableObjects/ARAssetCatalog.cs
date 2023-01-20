using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ARAssetCatalog
{
    public List<LanguageARAsset> assets;

    public ARAssetCatalog(List<LanguageARAsset> assets)
    {
        this.assets = assets;
    }

    public override string ToString()
    {
        if (assets == null) return "catalog has no asset list";
        if(assets.Count > 0)
        return $"catalog has {assets.Count} assets, first one being {assets[0].asset}";
        else return "catalog has 0 assets";
    }
}
