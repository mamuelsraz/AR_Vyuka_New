using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AREnabledManager : MonoBehaviour
{
    public ARSession session;
    public Page[] enabledInPages;

    private void Update()
    {
        bool enabled = false;
        foreach (var page in enabledInPages)
        {
            if (page.active)
            {
                enabled = true;
                break;
            }
        }
        if (enabled && !session.isActiveAndEnabled) session.enabled = true;

        if (!enabled && session.isActiveAndEnabled) session.enabled = false;
    }
}
