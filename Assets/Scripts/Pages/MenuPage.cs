using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : Page
{
    public SelectPage page;
    public void Show(bool chemistry) {
        page.loadChemie = chemistry;
        GoTo(page);
    }
}
