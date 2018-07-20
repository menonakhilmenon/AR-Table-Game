using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public static GameController gameController;

    public Dictionary<int, PlayerManager> Players;

    public GameObject slider; 
    public Transform parentTransform;

    [HideInInspector]
    public SkillManager localSkillManager;

    public Image[] skillCDSprite;
    public Image[] skillSprite;

    public
    
    #region Singleton Code

    void Awake()
    {
        if (gameController == null)
        {
            gameController = this;
        }
        else
        {
            Destroy(this);
        }
        Players = new Dictionary<int, PlayerManager>();
    }

    #endregion

    public void AttemptSKill(int skillIndex)
    {
        localSkillManager.AttemptSkillUse(skillIndex);
    }

    public void RegisterPlayer(int playerID,PlayerManager manager)
    {
        Players.Add(playerID, manager);
    }
    public void UnRegisterPlayer(int playerID)
    {
        Players.Remove(playerID);
    }
    public PlayerManager GetPlayer(int playerID)
    {
        return Players[playerID];
    }
}
