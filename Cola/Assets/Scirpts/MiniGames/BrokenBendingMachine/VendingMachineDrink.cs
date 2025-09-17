using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineDrink : MonoBehaviour
{
    void Start()
    {
        // 5초 뒤에 자동으로 파괴 (어딘가에 끼는 현상 방지)
        Destroy(gameObject, 5f);
    }

    // 다른 Collider와 충돌했을 때 호출됨
    private void OnCollisionEnter(Collision collision)
    {
        // "Floor" 태그를 가진 오브젝트와 부딪혔다면
        if (collision.gameObject.CompareTag("Floor"))
        {
            // 즉시 파괴
            Destroy(gameObject);
        }
    }
}
