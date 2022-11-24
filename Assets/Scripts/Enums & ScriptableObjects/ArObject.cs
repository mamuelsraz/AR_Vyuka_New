using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArObject", menuName = "ScriptableObjects/ARObject", order = 1)]
public class ArObject : ScriptableObject
{
    public GameObject obj;
    public string nickName;
    [HideInInspector] public string bundle;
    public string area;
    public string category;
    public Sprite sprite;
}
