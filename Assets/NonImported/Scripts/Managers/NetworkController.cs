using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkBehaviour {

    [HideInInspector]
    public static int playerNumber = 0;
    public static NetworkController networkController;


    static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    const string PLAYER_ID_PREFIX = "Player ";

    #region Singleton Assignment

    void Awake()
    {
        if (networkController == null)
        {
            networkController = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Player Register , UnRegister and GetPlayer

    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        playerNumber++;
        _player.playerID = playerNumber;
        _player.transform.name = _playerID;
        _player.playerNetworkID = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        playerNumber--;
        players.Remove(_playerID);
    }


    public static PlayerManager GetPlayer(string _playerID)
    {
        return players[_playerID];
    }


    #endregion

    //public void TakeDamage(int damage, string playerManager)
    //{
    //    RpcTakeDamage(damage, playerManager);
    //}

    //[ClientRpc]
    //private void RpcTakeDamage(int damage, string playerManagerID)
    //{
    //    PlayerManager playerManager = GetPlayer(playerManagerID);

    //    if (playerManager.isDead)
    //    {
    //        return;
    //    }
    //    playerManager.currentHealth -= damage;

    //    Debug.Log(transform.name + " has " + playerManager.currentHealth + " Health");
    //    if (playerManager.currentHealth <= 0)
    //    {
    //        playerManager.Die();
    //    }

    //}

}
