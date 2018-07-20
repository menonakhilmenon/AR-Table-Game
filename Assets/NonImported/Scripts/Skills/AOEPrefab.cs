using UnityEngine;

public class AOEPrefab : MonoBehaviour
{

    public int casterID=0;

    [SerializeField]
    float duration = 1f;
    [SerializeField]
    int damage = 5;

    void Start ()
    {
        Destroy(gameObject, duration);
	}

    private void OnTriggerEnter(Collider other)
    {
        CollRef collider;
        collider = other.gameObject.GetComponent<CollRef>();
        if (collider == null)
        {
            return;
        }
        else
        {
            if (collider.playerManager.playerID == casterID)
            {
                return;
            }
            else
            {
                collider.playerManager.TakeDamage(damage);
            }
        }
    }
}
