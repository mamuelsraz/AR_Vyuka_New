using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacePage : MonoBehaviour
{
    public Page page;
    public Page viewPage;
    public Camera camera;

    [SerializeField] GameObject placementIndicator;
    [SerializeField] Button placeButton;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    List<ARAnchor> m_AnchorPoints;

    public ARRaycastManager m_RaycastManager;
    public ARAnchorManager m_AnchorManager;
    public ARPlaneManager m_PlaneManager;

    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Awake()
    {
        m_AnchorPoints = new List<ARAnchor>();
    }

    void Update()
    {
        if (page.active)
        {


            if (!placementIndicator.activeSelf) placementIndicator.SetActive(true);

            UpdatePlacementPose();
            UpdatePlacementIndicator();

            //placeButton.interactable = placementPoseIsValid;
#if UNITY_EDITOR
            placeButton.interactable = true;
#endif
        }
        else
        {
            if (placementIndicator.activeSelf) placementIndicator.SetActive(false);
        }
    }

    public void TryPlace()
    {
#if UNITY_EDITOR
        SelectedArObjectManager.instance.SpawnCurrent(null);
        page.GoTo(viewPage);
        return;
#endif
        if (placementPoseIsValid) PlaceObject();
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = s_Hits.Count > 0;

        if (placementPoseIsValid)
        {
            placementPose = s_Hits[0].pose;

            var cameraForward = camera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            if (!placementIndicator.activeSelf) placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            if (placementIndicator.activeSelf) placementIndicator.SetActive(false);
        }
    }

    void PlaceObject()
    {
        var hitPose = s_Hits[0].pose;
        var hitTrackableId = s_Hits[0].trackableId;
        var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

        var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

        if (anchor == null)
        {
            Debug.Log("Error creating anchor.");
        }
        else
        {
            m_AnchorPoints.Add(anchor);
            SelectedArObjectManager.instance.SpawnCurrent(anchor.transform);

            page.GoTo(viewPage);
        }
    }
}
