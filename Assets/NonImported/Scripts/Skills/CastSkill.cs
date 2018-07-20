using UnityEngine;

[CreateAssetMenu(menuName ="DotSlash/CastSKill")]
public class CastSkill : Skill {

    [SerializeField]
    private float castTime;

    private SkillManager skillManager;
    [SerializeField]
    private Skill leadUpSkill;

    public override void Initialize(GameObject obj)
    {
        ;
    }
    public override void TriggerAbility(GameObject obj, int userID)
    {
        skillManager = obj.GetComponent<SkillManager>();
        skillManager.Cast(leadUpSkill,castTime);
    }
}
