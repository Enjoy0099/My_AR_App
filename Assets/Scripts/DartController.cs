using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DartController : MonoBehaviour
{
    public GameObject DartPrefab;
    public Transform DartThrowPoint;
    ARSessionOrigin aRSession;
    GameObject aRCam;
    private GameObject DartTemp;
    private Rigidbody rb;

    private void Start()
    {
        aRSession = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        aRCam = GameObject.Find("AR Camera").gameObject;
    }

    private void OnEnable()
    {
        PlaceObjectOnPlane.onPlacedObject += DartsInit;
    }

    private void OnDisable()
    {
        PlaceObjectOnPlane.onPlacedObject -= DartsInit;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if(Physics.Raycast(raycast, out raycastHit))
            {
                if(raycastHit.collider.CompareTag("dart"))
                {
                    raycastHit.collider.enabled = false;

                    DartTemp.transform.parent = aRSession.transform;
                }
            }
        }
    }

    void DartsInit()
    {
        StartCoroutine(WaitAndSpawnDart());
    }

    public IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(0.1f);
        DartTemp = Instantiate(DartPrefab, DartThrowPoint.position, aRCam.transform.localRotation);
        DartTemp.transform.parent = aRCam.transform;
        rb = DartTemp.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}
