using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleResize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (0.5f + 0.25f * Mathf.Sin(2*Time.time));
        transform.Rotate(new Vector3(0, 100 * Time.deltaTime, 0));
    }
}
