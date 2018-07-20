using System.Collections;
using UnityEngine;
using Photon;

[RequireComponent(typeof(PlayerMover))]
public class PlayerManager : Photon.MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    Behaviour[] disableOnDeath;

    [HideInInspector]
    public string playerNetworkID = "";
    public int maxHealth = 100;

    [SerializeField]
    GameObject selfIndicator;

    [SerializeField]
    UnityEngine.UI.Image healthIndicator;

    public PhotonView view;

    [HideInInspector]
    public bool isLocalPlayer = false;

    public int currentHealth = 100;
    public bool isDead = false;
    public int playerID=0;

    [SerializeField]
    Transform shotOrigin;

    Transform playerTransform;
    //[SerializeField]
    //LineRenderer line;

    private void Start()
    {
        playerID = view.viewID;
        if (!view.isMine)
            Destroy(selfIndicator);
        transform.name = "Player " + playerID;
        transform.parent = GameController.gameController.parentTransform;
        playerTransform = GetComponent<PlayerMover>().playerTransform;
        currentHealth = maxHealth;
        GameController.gameController.RegisterPlayer(view.viewID, this);
    }

    public void TakeDamage(int damage)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        object[] a = new object[2];
        a[0] = view.viewID;a[1] = damage;
        
        view.RPC("TakeDamage",PhotonTargets.All, a);
    }
    public void RpcTakeDamage(int damage)
    {

        if (isDead)
            return;

        currentHealth -= damage;

        healthIndicator.fillAmount = (float)currentHealth / (float)maxHealth;

        Debug.Log(transform.name + " has " + currentHealth + " Health");
        if (currentHealth <= 0)
        {
            Die();
        }
    }



    #region Debug Line Function

    public void DisplayLine(float range,float width,bool triggerExplosion)
    {
        
        Vector3 rayOrigin = shotOrigin.position;
        //line.SetPosition(0, rayOrigin);
        RaycastHit hit=new RaycastHit();
        Vector3 boxScale = new Vector3(width, 1.25f, 0.1f);
        if (Physics.BoxCast(rayOrigin, boxScale, playerTransform.forward, out hit, playerTransform.rotation, range))
        {
            if (triggerExplosion)
                StartCoroutine(IexplosionCoroutine(hit.point));
            //line.SetPosition(1, hit.point);
            Debug.Log(hit.transform.name);
        }
        else
        {
            //line.SetPosition(1, shotOrigin.transform.position + shotOrigin.transform.forward * range);
            StartCoroutine(IexplosionCoroutine(shotOrigin.transform.position + shotOrigin.transform.forward * range));
        }
        //StartCoroutine(LineCoroutine());
    }

    IEnumerator IexplosionCoroutine(Vector3 pos)
    {
        anim.SetTrigger("firePoint");
        yield return new WaitForSeconds(0.5f);
        Instantiate(explosion, pos, Quaternion.identity, GameController.gameController.parentTransform);
    }

    //IEnumerator LineCoroutine()
    //{
    //    line.enabled = true;
    //    yield return new WaitForSeconds(1f);
    //    line.enabled = false;
    //}

    #endregion



    public void Die()
    {
        foreach (Behaviour component in disableOnDeath)
        {
            component.enabled = false;
        }
        Collider[] colliders = GetComponents<Collider>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Collider CollToDisable in colliders)
        {
            CollToDisable.enabled = false;
        }
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        GameController.gameController.UnRegisterPlayer(view.viewID);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
