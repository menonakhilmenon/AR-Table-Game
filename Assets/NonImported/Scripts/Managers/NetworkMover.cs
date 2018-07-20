using UnityEngine;

public class NetworkMover : Photon.MonoBehaviour {

    [SerializeField]
    Transform player;

    Vector3 targetPosition;
    Quaternion targetRotation;
    [SerializeField]
    Animator anim;
    [SerializeField]
    float maxVelocity=20f;
    public float velocity;
    [SerializeField]
    float cutOff = 0.01f;


    [SerializeField]
    [Range(0,1f)]
    float lerpSmoothing = 0.5f;

    // Use this for initialization
    void Start ()
    {
        targetPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (velocity * Mathf.Sign(velocity) > cutOff)
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("Velocity", velocity / maxVelocity);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        //anim.SetFloat("Velocity", (velocity / maxVelocity));
        if (!photonView.isMine)
        {
            player.position = Vector3.Lerp(player.position, targetPosition, lerpSmoothing);
            player.rotation = Quaternion.Lerp(player.rotation, targetRotation, lerpSmoothing);
        }
        else
        {
            velocity = Vector3.Dot(player.GetComponent<Rigidbody>().velocity,player.forward);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(player.position);
            stream.SendNext(velocity);
            stream.SendNext(player.rotation);
        }
        else
        {
            targetPosition = (Vector3)stream.ReceiveNext();
            velocity = (float)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
