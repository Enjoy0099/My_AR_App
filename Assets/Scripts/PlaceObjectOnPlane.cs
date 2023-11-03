using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private Pose placementPos;
    private Transform placementTransform;
    private bool placementPoseIsValid = false;
    private bool isObjectPlaced = false;
    private TrackableId placedPlaneId = TrackableId.invalidId;

    ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public static event Action onPlacedObject;

    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (!isObjectPlaced)
        {
            UpdatePlacementPosition();
            UpdatePlacementIndicator();

            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

    private void UpdatePlacementPosition()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if(m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            placementPoseIsValid = s_Hits.Count > 0;

            if(placementPoseIsValid)
            {
                placementPos = s_Hits[0].pose;
                placedPlaneId = s_Hits[0].trackableId;

                var planeManager = GetComponent<ARPlaneManager>();
                ARPlane arPlane = planeManager.GetPlane(placedPlaneId);
                placementTransform = arPlane.transform;
            }
        }
    }
    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPos.position, placementTransform.rotation);

        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPos.position, placementTransform.rotation);
        onPlacedObject?.Invoke();
        isObjectPlaced = true;
        placementIndicator.SetActive(false);
    }


}
