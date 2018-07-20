using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[RequireComponent(typeof(PlayerManager))]
public class SkillManager : Photon.MonoBehaviour
{

    public Skill[] skill;
    [HideInInspector]
    public PlayerManager playerManager;
    public Transform skillOrigin;

    [HideInInspector]
    public Animator anim;

    public Transform arCam;

    private int MaxSkills;

    [SerializeField]
    private Skill[] skillsToInitialize;

    [SerializeField]
    private float[] skillCD;
    private float[] skillCDFull;

    #region Skill UI Images

    private Image[] skillCDSprite;
    private Image[] skillSprite;

    #endregion

    #region SkillCast Variables

    private PlayerMover mover;
    private GameObject castSliderObj;
    private Slider castSlider;

    #endregion

    public GameObject AOEIndicator;
    public GameObject FrontalIndicatorBase;
    public GameObject FrontalIndicator;

    private Skill tempSkill= null;
    private DoubleTapSkill currentDTSkill = null;
    private int tempSkillIndex= -1;

    private void Start()
    {
        

        castSliderObj = GameController.gameController.slider;
        castSlider = castSliderObj.GetComponent<Slider>();
        mover = GetComponent<PlayerMover>();
        playerManager = GetComponent<PlayerManager>();
        anim = playerManager.anim;
        MaxSkills = skill.Length;
        skillCD = new float[MaxSkills];
        skillCDFull = new float[MaxSkills];
        skillCDSprite = GameController.gameController.skillCDSprite;
        skillSprite = GameController.gameController.skillSprite;

        for (int i = 0; i < skillsToInitialize.Length; i++)
        {
            skillsToInitialize[i].Initialize(gameObject);
        }

        for (int i = 0; i < MaxSkills; i++)
        {
            if (playerManager.isLocalPlayer)
            {
                skillSprite[i].sprite = skill[i].skillSprite;
            }
        }
    }

    //[Client]
    public void AttemptSkillUse(int skillIndex)
    {
        
        if (skillCD[skillIndex] > 0)
        {
            return;
        }

        CmdTriggerSkill(gameObject, skillIndex);

        skillCD[skillIndex] = skill[skillIndex].skillCD+skill[skillIndex].baseCD;
        skillCDFull[skillIndex] = skillCD[skillIndex];
        for (int i = 0; i < MaxSkills; i++)
        {
            if (skillCD[i] < skill[skillIndex].baseCD)
            {
                skillCD[i] = skill[skillIndex].baseCD;
                skillCDFull[i] = skillCD[i];
            }
        }
    }

    private void Update()
    {
        
        if (playerManager.currentHealth <= 0)
        {
            if (!playerManager.isDead)
            {
                playerManager.Die();
            }
        }

        if (!playerManager.isLocalPlayer)
        {
            return;
        }

        #region Skill Cooldown

        for (int i = 0; i < MaxSkills; i++)
        {
            if (skillCDFull[i] == 0)
                continue;
            skillCD[i] = Mathf.Clamp(skillCD[i]- Time.deltaTime,0f,10000);
            if (skillCD[i] == 0)
            {
                skillCDFull[i] = 0;
                skillCDSprite[i].fillAmount = 1;
            }
            else
            {
                skillCDSprite[i].fillAmount = (skillCDFull[i]-skillCD[i]) / skillCDFull[i];
            }
        }

        #endregion
    }

    #region Assigning Local SkillManager

    public void SetLocalSkillManager()
    {
        GameController.gameController.localSkillManager = this;
    }

    #endregion

    #region Skill Networking Side

    //[Command]
    private void CmdTriggerSkill(GameObject obj,int skillIndex)
    {
        
        int id = playerManager.view.viewID;

        object[] a=new object[2];
        a[0] = id;a[1] = skillIndex;

        transform.GetComponent<PlayerManager>().view.RPC("RpcTriggerSkill", PhotonTargets.All, a);
    }
    
    //[ClientRpc]
    [PunRPC]
    void RpcTriggerSkill(int viewID,int skillIndex)
    {
            GameObject obj = GameController.gameController.GetPlayer(viewID).gameObject;
            skill[skillIndex].TriggerAbility(obj, playerManager.playerID);
    }

    #endregion

    #region DoubleTap Skill

    public void DoubleTapSkill(DoubleTapSkill dtskill)
    {
        if (currentDTSkill != null)
        {
            CancelSkill();
        }
        currentDTSkill = dtskill;

        for (int i = 0; i < MaxSkills; i++)
        {
            if (dtskill == skill[i])
            {
                tempSkill = skill[i];
                tempSkillIndex = i;
                skill[i] = dtskill.leadUpSkill;
                skillSprite[i].sprite = dtskill.leadUpSkill.skillSprite;
                break;
            }
        }
    }

    #endregion

    #region Skill Cancel and Reset

    public void CancelSkill()
    {
        if (currentDTSkill != null)
        {
            currentDTSkill.ClearTap(gameObject);
            ResetSkill();
        }
        if (playerManager.isLocalPlayer)
        {
            mover.enabled = true;
            castSlider.value = 0f;
            castSliderObj.SetActive(false);
            AOEIndicator.SetActive(false);
            FrontalIndicator.SetActive(false);

            for (int i = 0; i < MaxSkills; i++)
            {
                {
                    skillSprite[i].sprite = skill[i].skillSprite;
                }
            }

        }
        StopAllCoroutines();
    }


    public void ResetSkill()
    {
        currentDTSkill = null;
        skill[tempSkillIndex] = tempSkill;
        skillSprite[tempSkillIndex].sprite = skill[tempSkillIndex].skillSprite;
        tempSkillIndex = -1;
        tempSkill = null;
    }

    #endregion

    #region SkillCast Functions

    public void Cast(Skill skill,float t)
    {
        StartCoroutine(CastWait(skill,t));
    }

    public IEnumerator CastWait(Skill skill, float castTime)
    {
        anim.SetTrigger("spellIdle");
        float t = 0f;
        if (playerManager.isLocalPlayer)
        {
            castSliderObj.SetActive(true);
            mover.enabled = false;
        }
        GetComponent<PlayerMover>().rb.velocity = Vector3.zero;

        
        while (t < castTime)
        {
            castSlider.value = t / castTime;
            t += Time.deltaTime;
            yield return null;
        }
        skill.TriggerAbility(gameObject, playerManager.playerID);

        if (playerManager.isLocalPlayer)
        {
            mover.enabled = true;
            castSliderObj.SetActive(false);
        }
    }

    #endregion
}
