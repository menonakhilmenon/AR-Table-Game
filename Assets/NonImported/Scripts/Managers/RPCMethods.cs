using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCMethods : MonoBehaviour {

    [SerializeField]
    SkillManager skillManager;

    
    [PunRPC]
    void RpcTriggerSkill(int viewID, int skillIndex)
    {
        GameObject obj = GameController.gameController.GetPlayer(viewID).gameObject;
        skillManager.skill[skillIndex].TriggerAbility(obj, skillManager.playerManager.playerID);
    }

    [PunRPC]
    void TakeDamage(int viewID,int damage)
    {
        GameController.gameController.GetPlayer(viewID).RpcTakeDamage(damage);
    }
}
