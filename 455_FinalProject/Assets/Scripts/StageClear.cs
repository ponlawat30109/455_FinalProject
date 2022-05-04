using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;

public class StageClear : PunBehaviour
{
    private bool isClear;

    void Update()
    {
        isClear = Boss.instance.isClear;
        if (isClear)
        {
            Debug.Log("Stage Clear");
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
