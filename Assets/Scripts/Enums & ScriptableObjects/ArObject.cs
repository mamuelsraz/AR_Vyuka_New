using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArObject", menuName = "ScriptableObjects/ARObject", order = 1)]
public class ArObject : ScriptableObject
{
    public GameObject obj;
    public string nickName;
    public string bundle;
    public bool physics;
    public ArObjCategories category;
}
