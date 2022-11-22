using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public List<Material> materialsThatChange;
    public List<Material> colorMaterials;
    List<MaterialOnRenderer> materialsOnRenderers;

    public void Initialize()
    {
        GameObject obj = SelectedArObjectManager.instance.selectedObject;
        materialsOnRenderers = new List<MaterialOnRenderer>();

        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material mat = renderer.materials[0];
                bool matches = materialsThatChange.Where(p => p.name == mat.name).Count() > 0;
                if (matches)
                {
                    materialsOnRenderers.Add(new MaterialOnRenderer(renderer, i, mat));
                }
            }
        }
    }

    public void ChangeColor(int i)
    {
        foreach (var item in materialsOnRenderers)
        {
            if (i == -1)
                item.renderer.materials[item.index] = item.material;
            else
                item.renderer.materials[item.index] = colorMaterials[i];
        }
    }
}

class MaterialOnRenderer
{
    public MeshRenderer renderer;
    public int index;
    public Material material;

    public MaterialOnRenderer(MeshRenderer renderer, int index, Material mat)
    {
        this.renderer = renderer;
        this.index = index;
        this.material = mat;
    }
}
