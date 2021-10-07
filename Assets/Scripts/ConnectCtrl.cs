using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public enum RegionsCodes
{
    AUTO,
    CAE,
    EU,
    US,
    USW
}

public class ConnectCtrl : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    [SerializeField]
    private string regionCode = null;

    #endregion


    #region Private Fields

    string gameVersion = "1";
    const string RegionPrefKey = "defaultRegion";

    bool IsFiring;


    #endregion


    #region MonoBehaviour CallBacks


    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start()
    {

    }


    #endregion

    #region Private Methods
    void SetButton(bool state, string msg)
    {
        GameObject.Find("Button").GetComponentInChildren<Text>().text = msg;
        GameObject.Find("Button").GetComponent<Button>().enabled = state;
    }
    #endregion

    #region Public Methods


    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
            SetButton(false, "FINDING MATCH...");

        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            SetButton(false, "CONNECTING...");
        }
        
    }

    public void FindMatch()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public void SetRegion(int index)
    {
        RegionsCodes region = (RegionsCodes)index;
        if (region == RegionsCodes.AUTO)
            regionCode = null;
        else
            regionCode = region.ToString();

        Debug.Log("Region: " + regionCode);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        SetButton(true, "LETS BATTLE");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        SetButton(false, "WATING PLAYERS");



    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log(newPlayer.NickName+ " Se Ha unido al cuarto, Players: "+PhotonNetwork.CurrentRoom.PlayerCount);


        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Match");
        }
    }




    #endregion

}

