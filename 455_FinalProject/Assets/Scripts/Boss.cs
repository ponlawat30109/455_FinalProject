using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;

public class Boss : PunBehaviour
{
    public static Boss instance;

    public float hp = 500;
    private float correctHP;
    private bool correctClear;
    public bool isClear = false;
    // private float takePlayerDamage = 0;

    // private IEnumerator delayCoroutine;

    void Awake()
    {
        instance = this;

        // delayCoroutine = Delay();
    }

    private void Update()
    {
        // if (isClear == true)
        // {
        // StartCoroutine(Delay());
        // }

        TakeDamge();

        if (isClear)
        {
            Debug.Log("clear");
        }
    }

    void TakeDamge()
    {
        // Debug.Log(CharacterMovement.instance.playerDamage);
        // hp -= takePlayerDamage;
        // if (hp > 0)
        //     return;
        if (hp < 0)
        {
            Destroy(this.gameObject);
            isClear = true;
        }
    }

    // IEnumerator Delay()
    // {
    //     yield return new WaitForSeconds(2f);
    //     PhotonNetwork.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    // }

    // void SceneChange()
    // {
    //     PhotonNetwork.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    // }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         TakeDamge();
    //     }
    // }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         TakeDamge();
    //     }
    // }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(hp);
            stream.SendNext(isClear);
        }
        else
        {
            correctHP = (float)stream.ReceiveNext();
            correctClear = (bool)stream.ReceiveNext();
        }
    }
}
