using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartFade : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Fade", 1f);
        
    }

    void Fade() {
        image.DOFade(0, 0.5f).onComplete += () =>
        {
            Destroy(gameObject);
        };
    }
}
