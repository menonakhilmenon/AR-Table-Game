using UnityEngine;
using System.Collections;

public class KnightController : MonoBehaviour
{
    static Animator anim;
    public float speed = 10.0f;
    public float rotationspeed = 100.00f;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationspeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Firepoint");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Blastarea");
        }
        if(translation != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }
}