using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControl : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;
    public float trace_dist = 10;
    public float attack_dist = 2;
    public bool is_die = false;

    Transform monster_tr;
    Transform player_tr;
    NavMeshAgent agent;
    Animator anim;

    readonly int hash_trace = Animator.StringToHash("IsTrace");
    readonly int hash_attack = Animator.StringToHash("IsAttack");
    readonly int hash_hit = Animator.StringToHash("Hit");
    readonly int hash_player_die = Animator.StringToHash("PlayerDie");
    readonly int hash_speed = Animator.StringToHash("Speed");
    readonly int hash_die = Animator.StringToHash("Die");

    GameObject blood_effect;
    int hp = 100;

    void OnEnable()
    {
        PlayerControl.OnPlayerDie += this.OnPlayerDie;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    void OnDisable()
    {
        PlayerControl.OnPlayerDie -= this.OnPlayerDie;
    }

    void Awake()
    {
        monster_tr = GetComponent<Transform>();

        player_tr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        anim = GetComponent<Animator>();

        blood_effect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    void Update()
    {
        if(agent.remainingDistance >= 2)
        {
            Vector3 direction = agent.desiredVelocity;
            Quaternion rot = Quaternion.LookRotation(direction);
            monster_tr.rotation = Quaternion.Slerp(monster_tr.rotation,
                                                   rot, 
                                                   Time.deltaTime * 10);
        }
    }

    IEnumerator CheckMonsterState()
    {
        while(!is_die)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE)
                yield break;

            float distance = Vector3.Distance(player_tr.position, monster_tr.position);

            if(distance <= attack_dist)
            {
                state = State.ATTACK;
            }
            else if(distance <= trace_dist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while(!is_die)
        {
            switch(state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hash_trace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(player_tr.position);
                    agent.isStopped = false;
                    anim.SetBool(hash_trace, true);
                    anim.SetBool(hash_attack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hash_attack, true);
                    break;
                case State.DIE:
                    is_die = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hash_die);
                    GetComponent<CapsuleCollider>().enabled = false;

                    yield return new WaitForSeconds(3);

                    hp = 100;
                    is_die = false;
                    state = State.IDLE;

                    GetComponent<CapsuleCollider>().enabled = true;
                    this.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
        }
    }

    public void OnDamage(Vector3 pos, Vector3 normal)
    {
        anim.SetTrigger(hash_hit);

        Quaternion rot = Quaternion.LookRotation(normal);
        ShowBloodEffect(pos, rot);

        hp -= 30;
        if (hp <= 0)
        {
            state = State.DIE;

            GameManager.instance.DisplayScore(50);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(blood_effect, pos, rot, monster_tr);
        Destroy(blood, 1);
    }

    void OnDrawGizmos()
    {
        if(state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, trace_dist);
        }
        if(state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attack_dist);
        }
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();

        agent.isStopped = true;
        anim.SetFloat(hash_speed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hash_player_die);
    }
}