using UnityEngine;

public class TouchControls : MonoBehaviour
{
    float initialDistance = 0f;
    Vector2 initialRotatePos = Vector2.one;
    float initialRotation = 0f;
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
                initialRotation = obj.localEulerAngles.y;
            }
            else
            {
                Vector2 factor = initialRotatePos - touchZero.position;
                obj.localEulerAngles = new Vector3(0, initialRotation + factor.x, 0);
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
                initialScale = obj.localScale;
                initialRotation = obj.localEulerAngles.y;
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
