using UnityEngine;
using UnityEngine.UI;

public class SpecialMenuPage : Page
{
    public PlacePage placePage;
    public TextViewPage textViewPage;
    public ArObject textARObject;
    public ArObject imageARObject;
    public Image image;
    public Material material;
    public float idealImageScale = 0.2f;
    Vector2 dimensions = Vector2.one;

    public void PlaceText()
    {
        if (!AssetStreamingManager.instance.cachedArObjects.ContainsKey(textARObject))
        {
            var obj = Instantiate(textARObject.obj);
            obj.SetActive(false);
            AssetStreamingManager.instance.cachedArObjects.Add(textARObject, obj);
        }
        SelectedArObjectManager.instance.SelectNew(textARObject);

        placePage.viewPage = textViewPage;
        GoTo(placePage);
    }

    public void PlaceImage()
    {
        if (!AssetStreamingManager.instance.cachedArObjects.ContainsKey(imageARObject))
        {
            var obj = Instantiate(imageARObject.obj);
            obj.SetActive(false);
            AssetStreamingManager.instance.cachedArObjects.Add(imageARObject, obj);
        }
        SelectedArObjectManager.instance.SelectNew(imageARObject);

        var imageObj = SelectedArObjectManager.instance.selectedObject;
        imageObj.transform.GetChild(0).localScale = new Vector3(dimensions.x, dimensions.y, 1);

        placePage.viewPage = textViewPage;
        GoTo(placePage);
    }

    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            LoadImage(path);
        });

        Debug.Log("Permission result: " + permission);
    }

    void LoadImage(string path)
    {
        Debug.Log("Image path: " + path);
        if (path != null)
        {
            // Create Texture from selected image
            Texture2D texture = NativeGallery.LoadImageAtPath(path);
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + path);
                return;
            }
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            material.mainTexture = texture;

            dimensions = new Vector2(texture.width, texture.height);
            dimensions = dimensions.normalized;
            dimensions *= idealImageScale;
        }
    }
}
