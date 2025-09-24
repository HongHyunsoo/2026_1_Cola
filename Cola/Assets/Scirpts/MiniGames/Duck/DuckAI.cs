using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DuckAI : MonoBehaviour
{
    private enum DuckState { Patrolling, Chasing, Returning }
    private DuckState currentState;
    private bool isAngry = false;
    private bool isPlayerInChaseZone = false;

    [Header("타겟 및 이동 설정")]
    public Transform player;
    public Transform nestPoint;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("공격 설정")]
    [Tooltip("플레이어에게 닿았을 때 초당 뺏을 콜라(L)의 양")]
    public float attackDamagePerSecond = 5f;
    private float attackCooldown = 1.0f; // 1초의 공격 쿨타임
    private float nextAttackTime = 0f;   // 다음 공격이 가능한 시간

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(DuckState.Patrolling);
    }

    void Update()
    {
        switch (currentState)
        {
            case DuckState.Patrolling:
                if (isAngry && isPlayerInChaseZone) ChangeState(DuckState.Chasing);
                break;
            case DuckState.Chasing:
                if (player != null) agent.SetDestination(player.position);
                if (!isPlayerInChaseZone) ChangeState(DuckState.Returning);
                break;
            case DuckState.Returning:
                if (isPlayerInChaseZone) ChangeState(DuckState.Chasing);
                if (!agent.pathPending && agent.remainingDistance < 0.5f) ChangeState(DuckState.Patrolling);
                break;
        }
    }

    // ★★★ 유령이 계속 머무르는 것을 감지하는 방식으로 변경! ★★★
    private void OnTriggerStay(Collider other)
    {
        // 추격 중이고, 공격 쿨타임이 지났으며, 대상이 플레이어일 경우
        if (currentState == DuckState.Chasing && Time.time >= nextAttackTime && other.CompareTag("Player"))
        {
            nextAttackTime = Time.time + attackCooldown; // 다음 공격 시간을 1초 뒤로 설정
            GameManager.instance.LoseFuel(attackDamagePerSecond);
            Debug.Log("오리에게 계속 뜯기는 중!");
        }
    }

    private void ChangeState(DuckState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case DuckState.Patrolling:
                agent.speed = patrolSpeed;
                agent.SetDestination(nestPoint.position);
                break;
            case DuckState.Chasing:
                agent.speed = chaseSpeed;
                break;
            case DuckState.Returning:
                agent.speed = patrolSpeed;
                agent.SetDestination(nestPoint.position);
                break;
        }
    }

    public void TriggerChase()
    {
        isAngry = true;
        ChangeState(DuckState.Chasing);
    }

    public void OnPlayerEnterChaseZone() { isPlayerInChaseZone = true; }
    public void OnPlayerExitChaseZone() { isPlayerInChaseZone = false; }
}