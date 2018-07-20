using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMover : MonoBehaviour{

    [SerializeField]
    [Range(0f, 100f)]
    float speed=5f;

    [SerializeField]
    [Range(0f, 360f)]
    float tilt=5f;

    public Transform playerTransform;
    Vector3 moveHorizontal;
    float moveVertical;
    Vector3 addVel;
    [SerializeField]
    float gravity = 1f;

    [SerializeField]
    public Rigidbody rb;

    private void Start()
    {
        moveHorizontal = new Vector3
        {
            x = 0,
            y = 0,
            z = 0
        };
        addVel = Vector3.zero;
    }

    void FixedUpdate ()
    {
        moveHorizontal.y = CrossPlatformInputManager.GetAxisRaw("Horizontal")*tilt;// CrossPlatformInputManager.GetAxis("Horizontal") *tilt;
        moveVertical = CrossPlatformInputManager.GetAxisRaw("Vertical");//CrossPlatformInputManager.GetAxis("Vertical");
        if (moveVertical < 0)
        {
            moveVertical /= 2;
        }
        addVel.y = rb.velocity.y;
        rb.velocity = playerTransform.forward* moveVertical * speed +addVel;
        rb.AddForce(Vector3.down * gravity);
        playerTransform.localRotation = playerTransform.localRotation * Quaternion.Euler(moveHorizontal*Time.deltaTime);
    }
}
