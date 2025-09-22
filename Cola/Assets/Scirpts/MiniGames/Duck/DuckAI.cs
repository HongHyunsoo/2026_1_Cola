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
    private bool hasAttackedThisChase = false; // �̹� �߰ݿ��� ������ �ߴ��� ����

    [Header("Ÿ�� �� �̵� ����")]
    public Transform player;
    public Transform nestPoint;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("���� ����")]
    [Tooltip("�÷��̾�� ����� �� ���� �ݶ�(L)�� ��")]
    public float attackDamage = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(DuckState.Patrolling);
    }

    void Update()
    {
        // ���� ���¿� ���� �ٸ� �ൿ�� �����մϴ�.
        switch (currentState)
        {
            case DuckState.Patrolling:
                // ȭ�� ����, �÷��̾ �ٽ� ������ ������ �߰�
                if (isAngry && isPlayerInChaseZone)
                {
                    ChangeState(DuckState.Chasing);
                }
                break;

            case DuckState.Chasing:
                // �÷��̾ ����ִٸ� ��� �߰�
                if (player != null)
                {
                    agent.SetDestination(player.position);
                }
                // �÷��̾ ������ ����� ���� ����
                if (!isPlayerInChaseZone)
                {
                    ChangeState(DuckState.Returning);
                }
                break;

            case DuckState.Returning:
                // ���� �߿� �÷��̾ �ٽ� ������ ������ ��� �߰�
                if (isPlayerInChaseZone)
                {
                    ChangeState(DuckState.Chasing);
                }
                // ������ ���� �� �����ߴٸ� ���� ���·� ����
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    ChangeState(DuckState.Patrolling);
                }
                break;
        }
    }

    // �÷��̾�� �浹���� �� ȣ��Ǵ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ����� �α� �߰� ����
        Debug.Log("������ ���𰡿� �浹�߽��ϴ�! ���: " + collision.gameObject.name);

        // �߰� ���̰�, ���� ������ ���� ������, �ε��� ����� �÷��̾��� ���
        if (currentState == DuckState.Chasing && !hasAttackedThisChase && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�浹 ����� �÷��̾�! ������ �����մϴ�!"); // ���� ����� �α� �߰� ����
            hasAttackedThisChase = true; // �����ߴٰ� ǥ��
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
                hasAttackedThisChase = false; // �ڡڡ� �߰��� ������ ������ ���� ��ȸ �ʱ�ȭ! �ڡڡ�
                break;
            case DuckState.Returning:
                agent.speed = patrolSpeed;
                agent.SetDestination(nestPoint.position);
                break;
        }
    }

    // ���� �ٽ�: �ݶ� ���� ȣ���� �� �ϳ��� �Լ�! ����
    public void TriggerChase()
    {
        isAngry = true;
        // ���� ���°� �����̵�, ��� �߰� ���·� ����!
        ChangeState(DuckState.Chasing);
        Debug.Log("������ ��ſ��� �г��Ͽ� �߰��� �����մϴ�!");
    }

    public void OnPlayerEnterChaseZone() { isPlayerInChaseZone = true; }
    public void OnPlayerExitChaseZone() { isPlayerInChaseZone = false; }
}