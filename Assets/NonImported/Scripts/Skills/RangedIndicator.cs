using UnityEngine;

[CreateAssetMenu(menuName ="DotSlash/Indicators/RangedIndicator")]
public class RangedIndicator : DoubleTapSkill {

    public float range;
    public float width;

    public override void TriggerAbility(GameObject obj, int userID)
    {
        base.TriggerAbility(obj, userID);
        GameObject indicator;
        indicator = obj.GetComponent<SkillManager>().FrontalIndicator;
        indicator.GetComponent<RectTransform>().localScale = new Vector3(width, range/2);
        obj.GetComponent<SkillManager>().FrontalIndicatorBase.SetActive(true);
        indicator.SetActive(true);
    }
    public override void ClearTap(GameObject obj)
    {
        GameObject indicator;
        indicator = obj.GetComponent<SkillManager>().FrontalIndicator;
        obj.GetComponent<SkillManager>().FrontalIndicatorBase.SetActive(false);
        indicator.SetActive(false);
    }
}
