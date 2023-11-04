using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Dart : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject dirObj;
    public bool isForceOK = false;
    bool isDartRotating = false;
    bool isDartReadyToShoot = true;
    bool isDartHitOnBoard = false;

    ARSessionOrigin aRSession;
    GameObject aRCam;

    public Collider dartFrontCollider;

    private void Start()
    {
        aRSession = GameObject.FindGameObjectWithTag("AR Session Origin").GetComponent<ARSessionOrigin>();
        aRCam = GameObject.Find("AR Camera").gameObject;

        if(TryGetComponent(out Rigidbody rigid))
        rb = rigid;
        dirObj = GameObject.FindGameObjectWithTag("DartThrowPoint");
    }

    private void FixedUpdate()
    {
        if(isForceOK)
        {
            dartFrontCollider.enabled = true;
            StartCoroutine(InitDartDestroyVFX());
            GetComponent<Rigidbody>().isKinematic = false;
            isForceOK = false;
            isDartReadyToShoot = true;
        }

        rb.AddForce(dirObj.transform.forward * (12f + 6f) * Time.deltaTime, ForceMode.VelocityChange);

        if(isDartReadyToShoot)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 20f);
        }

        if(isDartRotating)
        {
            isDartRotating = false;
            transform.Rotate(Vector3.forward * Time.deltaTime * 400f);
        }
    }

    IEnumerator InitDartDestroyVFX()
    {
        yield return new WaitForSeconds(5f);
        if(!isDartHitOnBoard)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("dart_board"))
        {
            Handheld.Vibrate();

            GetComponent<Rigidbody>().isKinematic = true;
            isDartRotating = false;

            isDartHitOnBoard = true;
        }
    }
}
