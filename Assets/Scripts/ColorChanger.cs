using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static ColorChanger instance;
    public List<Material> materialsThatChange;
    public List<Material> colorMaterials;
    List<MaterialOnRenderer> materialsOnRenderers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

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
                string matName = mat.name.Replace(" (Instance)", "");
                Debug.Log(matName);
                bool matches = materialsThatChange.Where(p => p.name == matName).Count() > 0;
                if (matches)
                {
                    materialsOnRenderers.Add(new MaterialOnRenderer(renderer, i, mat));
                }
            }
        }

        ChangeColor(-1);
    }

    public void ChangeColor(int i)
    {
        foreach (var item in materialsOnRenderers)
        {
            Material[] materials = item.renderer.materials;
            if (i == -1)
                materials[item.index] = item.material;
            else
                materials[item.index] = colorMaterials[i];
            item.renderer.materials = materials;
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
