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
    private bool hasAttackedThisChase = false; // 이번 추격에서 공격을 했는지 여부

    [Header("타겟 및 이동 설정")]
    public Transform player;
    public Transform nestPoint;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("공격 설정")]
    [Tooltip("플레이어에게 닿았을 때 뺏을 콜라(L)의 양")]
    public float attackDamage = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(DuckState.Patrolling);
    }

    void Update()
    {
        // 현재 상태에 따라 다른 행동을 수행합니다.
        switch (currentState)
        {
            case DuckState.Patrolling:
                // 화가 났고, 플레이어가 다시 영역에 들어오면 추격
                if (isAngry && isPlayerInChaseZone)
                {
                    ChangeState(DuckState.Chasing);
                }
                break;

            case DuckState.Chasing:
                // 플레이어가 살아있다면 계속 추격
                if (player != null)
                {
                    agent.SetDestination(player.position);
                }
                // 플레이어가 영역을 벗어나면 복귀 시작
                if (!isPlayerInChaseZone)
                {
                    ChangeState(DuckState.Returning);
                }
                break;

            case DuckState.Returning:
                // 복귀 중에 플레이어가 다시 영역에 들어오면 즉시 추격
                if (isPlayerInChaseZone)
                {
                    ChangeState(DuckState.Chasing);
                }
                // 둥지에 거의 다 도착했다면 순찰 상태로 변경
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    ChangeState(DuckState.Patrolling);
                }
                break;
        }
    }

    // 플레이어와 충돌했을 때 호출되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // ▼▼▼ 디버그 로그 추가 ▼▼▼
        Debug.Log("오리가 무언가와 충돌했습니다! 대상: " + collision.gameObject.name);

        // 추격 중이고, 아직 공격한 적이 없으며, 부딪힌 대상이 플레이어일 경우
        if (currentState == DuckState.Chasing && !hasAttackedThisChase && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("충돌 대상은 플레이어! 공격을 실행합니다!"); // ▼▼▼ 디버그 로그 추가 ▼▼▼
            hasAttackedThisChase = true; // 공격했다고 표시
            GameManager.instance.LoseFuel(attackDamage);
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
                hasAttackedThisChase = false; // ★★★ 추격을 시작할 때마다 공격 기회 초기화! ★★★
                break;
            case DuckState.Returning:
                agent.speed = patrolSpeed;
                agent.SetDestination(nestPoint.position);
                break;
        }
    }

    // ▼▼▼ 핵심: 콜라가 직접 호출할 단 하나의 함수! ▼▼▼
    public void TriggerChase()
    {
        isAngry = true;
        // 현재 상태가 무엇이든, 즉시 추격 상태로 변경!
        ChangeState(DuckState.Chasing);
        Debug.Log("오리가 당신에게 분노하여 추격을 시작합니다!");
    }

    public void OnPlayerEnterChaseZone() { isPlayerInChaseZone = true; }
    public void OnPlayerExitChaseZone() { isPlayerInChaseZone = false; }
}