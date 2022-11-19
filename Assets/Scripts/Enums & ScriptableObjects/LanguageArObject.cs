using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageArObject", menuName = "ScriptableObjects/LanguageARObject", order = 2)]
public class LanguageArObject : ArObject
{
    public LanguageBlock[] nameInLanguage;

    public LanguageBlock GetBlock(string language) {
        LanguageBlock block = null;
        foreach (var item in nameInLanguage)
        {
            if (item.language == language) {
                block = item;
                break;
            }
        }

        return block;
    }
}
