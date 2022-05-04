using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private string charPrefName;
    [SerializeField] Transform spawnPoint;

    private GameObject ownerChar;

    void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.inRoom)
        {
            if (PhotonNetwork.room.PlayerCount == 2)
            {
                if (ownerChar == null)
                {
                    // PhotonNetwork.LoadLevel(1);
                    charPrefName = DatabaseConnection.instance.heroName;
                    ownerChar = PhotonNetwork.Instantiate(charPrefName, spawnPoint.position, spawnPoint.rotation, 0);
                }
            }
        }
    }
}
