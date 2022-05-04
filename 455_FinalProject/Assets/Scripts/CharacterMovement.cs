using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;

public class CharacterMovement : PunBehaviour, IPunObservable
{
    public static CharacterMovement instance;

    [SerializeField] float jump;
    [SerializeField] float speed = 5f;
    public float playerDamage = 0;

    [SerializeField] LayerMask ground;
    [SerializeField] Rigidbody2D _rigidbody;
    // [SerializeField] Collider2D _collider;

    private Animator animator;

    // private bool isJump = false;

    // [SerializeField] bool IsGround() => Physics2D.IsTouchingLayers(_collider, ground);

    // private Vector3 scaleL = new Vector3(-0.8f, 0.8f, 0.8f);
    // private Vector3 scaleR = new Vector3(0.8f, 0.8f, 0.8f);

    private Vector3 correctPosition;
    private Quaternion correctRotation;
    private GameObject[] enemyList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        if (photonView.isMine)
        {
            Movement();
            Attack();
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, correctPosition, 5f * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, correctRotation, 5f * Time.deltaTime);
        }
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jump * 5);
        }

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position += movement * Time.deltaTime * speed;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttack", true);
            Debug.Log("Attack");

            foreach (var enemy in enemyList)
            {
                // Debug.Log(this.gameObject.transform.position);
                // Debug.Log(enemy.transform.position);

                if (Vector2.Distance(this.gameObject.transform.position, enemy.transform.position) <= 4)
                {
                    playerDamage = 50;
                    if (enemy != null)
                    {
                        Boss.instance.hp -= playerDamage;
                    }

                }
                else
                {
                    playerDamage = 0;
                    if (enemy != null)
                    {
                        Boss.instance.hp -= playerDamage;
                    }
                }

                Debug.Log("playerDamge : " + playerDamage);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isAttack", false);
            Debug.Log("Attack");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            correctPosition = (Vector3)stream.ReceiveNext();
            correctRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
