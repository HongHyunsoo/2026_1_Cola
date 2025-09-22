using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckChaseZone : MonoBehaviour
{
    [Tooltip("�� ������ ����� ���� AI")]
    public DuckAI linkedDuck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ ���Դٰ� �������� �˸�
            linkedDuck.OnPlayerEnterChaseZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ �����ٰ� �������� �˸�
            linkedDuck.OnPlayerExitChaseZone();
        }
    }
}