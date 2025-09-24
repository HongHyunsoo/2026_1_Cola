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

    [Header("Ÿ�� �� �̵� ����")]
    public Transform player;
    public Transform nestPoint;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("���� ����")]
    [Tooltip("�÷��̾�� ����� �� �ʴ� ���� �ݶ�(L)�� ��")]
    public float attackDamagePerSecond = 5f;
    private float attackCooldown = 1.0f; // 1���� ���� ��Ÿ��
    private float nextAttackTime = 0f;   // ���� ������ ������ �ð�

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

    // �ڡڡ� ������ ��� �ӹ����� ���� �����ϴ� ������� ����! �ڡڡ�
    private void OnTriggerStay(Collider other)
    {
        // �߰� ���̰�, ���� ��Ÿ���� ��������, ����� �÷��̾��� ���
        if (currentState == DuckState.Chasing && Time.time >= nextAttackTime && other.CompareTag("Player"))
        {
            nextAttackTime = Time.time + attackCooldown; // ���� ���� �ð��� 1�� �ڷ� ����
            GameManager.instance.LoseFuel(attackDamagePerSecond);
            Debug.Log("�������� ��� ���� ��!");
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