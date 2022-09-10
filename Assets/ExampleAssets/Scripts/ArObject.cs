using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArObject", menuName = "ScriptableObjects/ARObject", order = 1)]
public class ArObject : ScriptableObject
{
    public string bundle;
    public string nickName;
    public ArObjCategories category;
}
