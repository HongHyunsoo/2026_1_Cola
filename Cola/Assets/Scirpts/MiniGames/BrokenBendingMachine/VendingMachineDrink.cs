using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineDrink : MonoBehaviour
{
    void Start()
    {
        // 5�� �ڿ� �ڵ����� �ı� (��򰡿� ���� ���� ����)
        Destroy(gameObject, 5f);
    }

    // �ٸ� Collider�� �浹���� �� ȣ���
    private void OnCollisionEnter(Collision collision)
    {
        // "Floor" �±׸� ���� ������Ʈ�� �ε����ٸ�
        if (collision.gameObject.CompareTag("Floor"))
        {
            // ��� �ı�
            Destroy(gameObject);
        }
    }
}
