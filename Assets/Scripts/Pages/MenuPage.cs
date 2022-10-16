using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : Page
{
    public Page page;
    public void Show(string area) {
        SelectedArObjectManager.instance.selectedArea = area; 
        GoTo(page);
    }
}
