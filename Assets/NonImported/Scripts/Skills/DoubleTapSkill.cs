using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTapSkill : Skill {

    [SerializeField]
    public Skill leadUpSkill;

    private SkillManager skillManager;

    public override void Initialize(GameObject obj)
    {
        return;
    }

    public override void TriggerAbility(GameObject obj, int userID)
    {
        skillManager = obj.GetComponent<SkillManager>();
        skillManager.DoubleTapSkill(this);
    }
    public virtual void ClearTap(GameObject obj)
    {
        ;
    }
    
}
