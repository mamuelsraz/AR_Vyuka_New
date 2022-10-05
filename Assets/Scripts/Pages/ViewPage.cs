using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPage : Page
{
    public TouchControls touchControls;

    private void Update()
    {
        if (active)
        {
            touchControls.RotateAndScaleObj(SelectedArObjectManager.instance.selectedObject.transform);
        }
    }

   
}
