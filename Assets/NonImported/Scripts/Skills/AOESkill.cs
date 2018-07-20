using UnityEngine;

[CreateAssetMenu(menuName = "DotSlash/AOESkill")]
public class AOESkill : Skill
{
    [SerializeField]
    GameObject AOEPrefab;
    [SerializeField]
    float range=1f;
    Transform parentTransform;

    public override void Initialize(GameObject obj)
    {
        ;
    }

    public override void TriggerAbility(GameObject obj,int userID)
    {
        Transform AOESpawnLocation = obj.GetComponent<PlayerMover>().playerTransform;
        parentTransform = GameController.gameController.parentTransform;
        obj.GetComponent<SkillManager>().anim.SetTrigger("blastArea");

        AOEPrefab AOE = Instantiate(AOEPrefab, AOESpawnLocation.position, AOESpawnLocation.rotation).GetComponent<AOEPrefab>();
        //NetworkController.networkController.CmdSpawnObject(AOE.gameObject);
        AOE.transform.parent = parentTransform;
        AOE.transform.localScale = new Vector3(range, range, range);
        AOE.casterID = userID;
        obj.GetComponent<SkillManager>().CancelSkill();
    }

}
