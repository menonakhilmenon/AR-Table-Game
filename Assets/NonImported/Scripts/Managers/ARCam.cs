using UnityEngine;
using Vuforia;

public class ARCam : MonoBehaviour {

    [SerializeField]
    Vector3 zeroPos;
    [SerializeField]
    Vector3 zeroRot;
    [SerializeField]
    Transform selfCanvas;
    // Use this for initialization
    void OnEnable ()
    {
        transform.position = zeroPos;
        transform.rotation = Quaternion.Euler(zeroRot);
	}
    private void Update()
    {
        selfCanvas.LookAt(transform);
    }
}
