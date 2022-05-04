using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // PhotonNetwork.LoadLevel(1);
        // PhotonNetwork.LeaveRoom();
        // Application.Quit();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
        Application.Quit();
    }
}
