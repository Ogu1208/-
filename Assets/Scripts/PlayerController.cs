using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    [Header("coinfig player")]
    public float movementSpeed = 3f;
    private Vector3 direction;
    private bool isWalk;

    // Input
    private float horizontal;
    private float vertical;

    [Header("Attack Config")]
    public ParticleSystem fxAttack;
    public Transform hitBox;
    [Range(0.2f, 1f)]
    public float hitRange = 0.5f;
    public LayerMask hitMask;
    private bool isAttack;
    public Collider[] hitInfo;
    public int amountDmg;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        MoveCharacter();

        UpdateAnimator();

    }

    #region 내 함수
    void Inputs()
    {
        // 캐릭터 이동을 위한 수평 및 수직 입력값
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // "Fire1" 버튼이 눌릴 때 공격 애니메이션을 트리거
        if (Input.GetButtonDown("Fire1") && isAttack == false)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        fxAttack.Emit(50);

        hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);

        foreach(Collider c in hitInfo)
        {
            c.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
        }
    }

    // 정규화된 이동 방향 벡터 계산, 캐릭터 회전 및 걷기 상태 설정
    void MoveCharacter()
    {
        // 정규화된 이동 방향 벡터 계산
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 캐릭터를 이동 방향으로 회전시키고 걷는 상태 설정
        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            isWalk = true;
        }
        else
        {
            // 움직임이 없을 때 걷는 상태를 false
            isWalk = false;
        }

        // 캐릭터 이동
        controller.Move(direction * movementSpeed * Time.deltaTime);
    }

    // 애니메이터 걷기 상태 업데이트
    void UpdateAnimator()
    {
        anim.SetBool("isWalk", false);
    }

    void AttackIsDone()
    {
        isAttack = false;
    }

    #endregion


    private void OnDrawGizmosSelected()
    {
        if(hitBox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position, hitRange);
        }
    }

}
