using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSetter :MonoBehaviour {

    [SerializeField]
    Transform[] startTransforms;

	void Start () {
        Connect();
	}

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("DotSlash");
    }
    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    void OnJoinedLobby()
    {
        Debug.Log("Lobby Joined");
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Random Joining Failed");
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        SpawnMyPlayer();
    }

    void SpawnMyPlayer()
    {
        GameObject obj;
        Transform startTransform = startTransforms[Random.Range(0, startTransforms.Length)];
        obj=PhotonNetwork.Instantiate("Player", startTransform.position, startTransform.rotation, 0);
        obj.GetComponentInChildren<PlayerManager>().isLocalPlayer = true;
        obj.GetComponentInChildren<PlayerNetworkSetup>().Initialize();
    }
}
