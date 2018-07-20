using UnityEngine;

public abstract class Skill : ScriptableObject
{

    public string skillName = "New Ability";
    public Sprite skillSprite;
    public float skillCD = 1f;
    public float baseCD = 0.5f;

    public abstract void Initialize(GameObject obj);

    public abstract void TriggerAbility(GameObject obj, int userID);

}
