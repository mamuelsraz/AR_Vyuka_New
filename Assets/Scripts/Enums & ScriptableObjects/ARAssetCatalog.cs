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
}
