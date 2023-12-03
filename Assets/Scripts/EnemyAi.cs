using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAi : MonoBehaviour
{

    private GameManager _GameManager;  // _는 외부 스크립트 의미

    private Animator anim;
    public ParticleSystem hitEffect;

    public int HP;
    private bool isDie;

    public enemyState state;

    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;

    // AI
    private NavMeshAgent agent;
    private int idWayPoint;
    private Vector3 destination;


    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
    }


    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(2.5f);  // 2.5초 동안 Die 애니메이션 하도록 기다림
        Destroy(this.gameObject);
    }

    #region MyRegion

    void GetHit(int amount)
    {
        if (isDie == true) { return;  }
        HP -= amount;

        if (HP > 0)
        {
            anim.SetTrigger("GetHit");
            hitEffect.Emit(25);
        }
        else
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
        
    }

    void StateManager()
    {
        switch(state)
        {
            case enemyState.IDLE:

                break;
            case enemyState.ALERT:
                break;

            case enemyState.EXPLORE:
                break;

            case enemyState.FOLLOW:
                break;

            case enemyState.FURY:
                break;

            case enemyState.PATROL:
                break;

        }
    }

    void ChangeState(enemyState newState)
    {
        StopAllCoroutines();  // 모든 루틴 중지
        state = newState;
        print(newState);

        switch (state)
        {
            case enemyState.IDLE:
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");
                break;

            case enemyState.ALERT:

                break;

            case enemyState.PATROL:

                idWayPoint = Random.Range(0, _GameManager.enemyWayPoints.Length);
                destination = _GameManager.enemyWayPoints[idWayPoint].position;
                agent.destination = destination;

                StartCoroutine("PATROL");
                break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(idleWaitTime);
        if(Rand() < 50)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
    }

    IEnumerator PATROL()
    {
        yield return new WaitForSeconds(patrolWaitTime);
        
        ChangeState(enemyState.IDLE);
    }

    int Rand()
    {
        int rand = Random.Range(0, 100); /// 0 ~ 99
        return rand;
    }

    #endregion
}
