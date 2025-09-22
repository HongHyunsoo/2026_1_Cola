using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckNest : MonoBehaviour
{
    [Tooltip("이 둥지를 지키는 오리 AI")]
    public DuckAI guardianDuck;
    private bool hasBeenRobbed = false; // 도둑이 한 번이라도 들었는지 여부

    // 둥지의 콜라가 훔쳐졌을 때 이 함수를 호출합니다.
    public void NotifyColaStolen()
    {
        // 아직 도둑이 든 적이 없다면 (최초의 도둑질이라면)
        if (!hasBeenRobbed)
        {
            hasBeenRobbed = true; // 이제 이 둥지는 도둑맞은 상태가 됩니다.
            // 오리에게 즉시 추격을 명령합니다!
            if (guardianDuck != null)
            {
                guardianDuck.TriggerChase();
            }
        }
    }
}