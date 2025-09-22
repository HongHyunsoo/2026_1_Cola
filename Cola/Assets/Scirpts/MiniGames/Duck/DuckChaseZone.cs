using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckChaseZone : MonoBehaviour
{
    [Tooltip("이 구역과 연결된 오리 AI")]
    public DuckAI linkedDuck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 들어왔다고 오리에게 알림
            linkedDuck.OnPlayerEnterChaseZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 나갔다고 오리에게 알림
            linkedDuck.OnPlayerExitChaseZone();
        }
    }
}