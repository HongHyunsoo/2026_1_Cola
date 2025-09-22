using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckNest : MonoBehaviour
{
    [Tooltip("�� ������ ��Ű�� ���� AI")]
    public DuckAI guardianDuck;
    private bool hasBeenRobbed = false; // ������ �� ���̶� ������� ����

    // ������ �ݶ� �������� �� �� �Լ��� ȣ���մϴ�.
    public void NotifyColaStolen()
    {
        // ���� ������ �� ���� ���ٸ� (������ �������̶��)
        if (!hasBeenRobbed)
        {
            hasBeenRobbed = true; // ���� �� ������ ���ϸ��� ���°� �˴ϴ�.
            // �������� ��� �߰��� ����մϴ�!
            if (guardianDuck != null)
            {
                guardianDuck.TriggerChase();
            }
        }
    }
}