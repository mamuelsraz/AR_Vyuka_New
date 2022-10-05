using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingGraphic : MonoBehaviour
{
    public float speed;
    float realSpeed;

    private void Start()
    {
        StartLoading();
    }

    private void Update()
    {
        transform.Rotate(Vector3.fwd, realSpeed * Time.deltaTime);
    }

    void StartLoading() {
        realSpeed = speed;
    }

    void StopLoading() { 
    
    }
}
