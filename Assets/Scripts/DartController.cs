using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DartController : MonoBehaviour
{
    public GameObject DartPrefab;
    public Transform DartThrowPoint;
    ARSessionOrigin aRSession;
    GameObject aRCam;
    Transform DartboardObj;
    private GameObject DartTemp;
    private Rigidbody rb;
    private bool isDartBoardSearched = false;
    private float m_distanceFromDartBoard = 0;
    public TMP_Text text_distance; 

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
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("dart"))
                {
                    raycastHit.collider.enabled = false;
                    DartTemp.transform.parent = aRSession.transform;

                    Dart currentDartScript = DartTemp.GetComponent<Dart>();
                    currentDartScript.isForceOK = true;

                    DartsInit();
                }
            }
        }

        if (isDartBoardSearched)
        {
            m_distanceFromDartBoard = Vector3.Distance(DartboardObj.position, aRCam.transform.position);
            text_distance.text = m_distanceFromDartBoard.ToString().Substring(0, 3);
        }
    }

    void DartsInit()
    {
        DartboardObj = GameObject.FindWithTag("dart_board").transform;
        if(DartboardObj)
        {
            isDartBoardSearched = true;
        }
        StartCoroutine(WaitAndSpawnDart());
    }

    public IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(1f);
        DartTemp = Instantiate(DartPrefab, DartThrowPoint.position, aRCam.transform.localRotation);
        DartTemp.transform.parent = aRCam.transform;
        rb = DartTemp.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}
