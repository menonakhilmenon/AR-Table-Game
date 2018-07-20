using UnityEngine;

[CreateAssetMenu(menuName = "DotSlash/Indicators/AOEIndicator")]
public class AOEIndicator : DoubleTapSkill
{

    public float radius;

    public override void TriggerAbility(GameObject obj, int userID)
    {
        base.TriggerAbility(obj, userID);
        GameObject indicator;
        indicator = obj.GetComponent<SkillManager>().AOEIndicator;
        indicator.GetComponent<RectTransform>().localScale = new Vector3(radius*2, radius*2);
        indicator.SetActive(true);
    }
    public override void ClearTap(GameObject obj)
    {
        GameObject indicator;
        indicator = obj.GetComponent<SkillManager>().AOEIndicator;
        indicator.SetActive(false);
    }
}
