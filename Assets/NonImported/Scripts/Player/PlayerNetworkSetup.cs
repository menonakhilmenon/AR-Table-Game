using UnityEngine;

[RequireComponent(typeof(SkillManager))]
public class PlayerNetworkSetup : Photon.MonoBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    Behaviour[] componentsToKill;
    [SerializeField]
    PhotonView view;

    #region Layer Assignments

    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string localLayerName = "LocalPlayer";

    #endregion


    private void Start()
    {
        Initialize();
    }
    
    public void Initialize()
    {
        if (view.isMine)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
            
            for (int i = 0; i < componentsToKill.Length; i++)
            {
                if (componentsToKill[i] == null)
                    continue;
                Destroy(componentsToKill[i]);
                componentsToKill[i] = null;
            }

        }
        if (view.isMine)
        {
            AssignLocalLayerRecursively(gameObject, localLayerName);
            GetComponent<SkillManager>().SetLocalSkillManager();
        }
        else
        {
            AssignRemoteLayerRecursively(gameObject, remoteLayerName);
        }

    }
    private void OnDestroy()
    {
        //NetworkController.UnRegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString());
    }

    #region Layer Assignment Functions

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void AssignRemoteLayerRecursively(GameObject obj, string remoteLayerName)
    {
        obj.layer = LayerMask.NameToLayer(remoteLayerName);
        foreach (Transform child in obj.transform)
        {
            AssignRemoteLayerRecursively(child.gameObject, remoteLayerName);
        }
    }
    void AssignLocalLayerRecursively(GameObject obj, string localLayerName)
    {
        obj.layer = LayerMask.NameToLayer(localLayerName);
        foreach (Transform child in obj.transform)
        {
            AssignLocalLayerRecursively(child.gameObject, localLayerName);
        }
    }

    void AssignLocalLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(localLayerName);
    }

    #endregion

}
