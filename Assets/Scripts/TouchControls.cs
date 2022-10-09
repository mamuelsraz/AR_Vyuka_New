using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    float initialDistance = 0f;
    Vector2 initialRotatePos = Vector2.one;
    Vector3 initialScale = Vector3.one;
    public void RotateAndScaleObj(Transform obj)
    {
        if (Input.touchCount == 1)
        {
            var touchZero = Input.GetTouch(0);
            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled) return;

            if (touchZero.phase == TouchPhase.Began)
            {
                initialRotatePos = touchZero.position;
                initialScale = obj.localScale;
            }
            else
            {
                Vector2 factor = touchZero.position - initialRotatePos;
                obj.localEulerAngles = new Vector3(0, factor.x, 0);
            }
        }
        else if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return; // basically do nothing
            }

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
            }
            else // if touch is moved
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                //if accidentally touched or pinch movement is very very small
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return; // do nothing if it can be ignored where inital distance is very close to zero
                }

                var factor = currentDistance / initialDistance;
                obj.localScale = initialScale * factor; // scale multiplied by the factor we calculated
            }
        }
    }
}
