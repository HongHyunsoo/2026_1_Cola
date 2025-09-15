using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // 플레이어가 이 구역에 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetPlayerSafeAreaStatus(true);
        }
    }

    // 플레이어가 이 구역에서 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetPlayerSafeAreaStatus(false);
        }
    }
}
