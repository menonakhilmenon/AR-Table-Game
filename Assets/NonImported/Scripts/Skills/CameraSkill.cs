using UnityEngine;

[CreateAssetMenu(menuName ="DotSlash/CameraSkill")]
public class CameraSkill : Skill {

    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    float range = 5f;

    public override void Initialize(GameObject obj)
    {
        ;
    }
    public override void TriggerAbility(GameObject obj, int userID)
    {
        SkillManager skillManager = obj.GetComponent<SkillManager>();
        Transform arCam = skillManager.arCam;
        float dist = Mathf.Clamp((arCam.position - GameController.gameController.parentTransform.position).magnitude*2,100f,10000f);
        RaycastHit hit;
        if (Physics.Raycast(arCam.position, arCam.forward, out hit, dist, LayerMask.NameToLayer("UI")))
        {
            GameObject aoe =  Instantiate(explosionPrefab, hit.point, Quaternion.identity, null);
            aoe.transform.localScale = new Vector3(range, range, range);
            aoe.transform.parent = GameController.gameController.parentTransform;
            aoe.GetComponent<AOEPrefab>().casterID = skillManager.GetComponent<PlayerManager>().view.viewID;
        }
    }
}
