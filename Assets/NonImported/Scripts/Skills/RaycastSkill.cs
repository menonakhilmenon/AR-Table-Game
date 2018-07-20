using UnityEngine;

[CreateAssetMenu(menuName ="DotSlash/RaycastSkill")]
public class RaycastSkill : Skill {

    public int damage = 1;
    public float range = 50f;

    [SerializeField]
    bool triggerExplosion=false;

    private float _range;

    public float width = 1f;
    private SkillManager skillManager;

    private Vector3 boxScale;
    private GameObject player;
    private Transform rayOrigin;
    //public float hitForce = 100f;

    public override void Initialize(GameObject obj)
    {
        _range = range * 2;
    }

    public override void TriggerAbility(GameObject obj,int userID)
    {

        skillManager = obj.GetComponent<SkillManager>();
        player = skillManager.gameObject;
        rayOrigin = skillManager.skillOrigin;

        
        player.GetComponent<PlayerManager>().DisplayLine(_range,width,triggerExplosion);
        RaycastHit hit;
        CollRef hitTarget = null;

        Vector3 boxScale = new Vector3(width, 1.25f, 0.1f);
        if (Physics.BoxCast(rayOrigin.position, boxScale, rayOrigin.forward, out hit, rayOrigin.rotation, _range))
        {
            hitTarget = hit.transform.GetComponent<CollRef>();
            if (hitTarget != null)
            {
                if (hitTarget.playerManager.playerID != userID)
                    hitTarget.playerManager.TakeDamage(damage);
            }
        }
        skillManager.CancelSkill();

    }

}
