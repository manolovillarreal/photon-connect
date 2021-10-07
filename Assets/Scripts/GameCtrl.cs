using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform Player1SpawnPoint;
    public Transform Player2SpawnPoint;
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.


    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Transform spawnPoint = (PhotonNetwork.IsMasterClient) ? Player1SpawnPoint : Player2SpawnPoint;
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
            GameObject.Find("CameraRig").GetComponent<CameraControl>().SetCameraTargets();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
