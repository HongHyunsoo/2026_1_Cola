using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI 설정")]
    public TextMeshProUGUI finalFuelText;
    public GameObject resultPanel; // 결과 UI를 담고 있는 패널

    [Header("플레이어 설정")]
    public GameObject playerObject; // 엔딩 씬에서 조종할 플레이어

    void Start()
    {
        // 결과 UI를 잠시 숨기고, 플레이어 조작을 활성화
        resultPanel.SetActive(false);
        playerObject.SetActive(true);

        // 커서를 다시 잠금 (플레이어가 움직여야 하므로)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 여기에 로켓이나 외계인과의 상호작용 로직을 추가할 수 있습니다.
        // 예를 들어, 특정 오브젝트에 다가가 상호작용하면 최종 결과를 보여주는 방식
        // 지금은 간단하게 3초 후에 결과가 나타나도록 해보겠습니다.
        Invoke("ShowFinalResult", 3f);
    }

    // 최종 결과를 보여주는 함수
    void ShowFinalResult()
    {
        // 플레이어 조작을 비활성화하고 커서를 품
        playerObject.GetComponent<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 결과 패널 활성화
        resultPanel.SetActive(true);

        // 파괴되지 않고 넘어온 GameManager 인스턴스를 찾습니다.
        if (GameManager.instance != null)
        {
            // GameManager에서 최종 연료 값을 가져와 UI에 표시합니다.
            float finalFuel = GameManager.instance.currentFuel;
            finalFuelText.text = "최종 수집 연료: " + finalFuel.ToString("F2") + " L"; // F2는 소수점 둘째 자리까지 표시
        }
        else
        {
            finalFuelText.text = "데이터를 불러오는 데 실패했습니다.";
        }
    }
}
