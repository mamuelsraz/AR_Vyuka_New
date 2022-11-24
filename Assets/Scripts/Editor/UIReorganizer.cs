using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIReorganizer
{
    static Vector2 offset = new Vector2(0, 0);
    static Vector2 size = new Vector2(1, 1);
    static int rowCount;

    [MenuItem("Tools/Reorganize/Divide")]
    static void Organize() {
        var pages = GameObject.FindGameObjectsWithTag("Page");
        for (int i = 0; i < pages.Length; i++)
        {
            int y = (i - (i % rowCount))/rowCount;
            int x = i - y * rowCount;

            Vector2 position = offset + new Vector2(x * size.x,y * size.y);

            var page = pages[i];
            page.transform.position = position;
        }
    }

    [MenuItem("Tools/Reorganize/Center")]
    static void Clamp() {
        var pages = GameObject.FindGameObjectsWithTag("Page");
        for (int i = 0; i < pages.Length; i++)
        {
            var page = pages[i];
            page.transform.position = Vector2.zero;
        }
    }
}
