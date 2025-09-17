using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 사용

public class BrokenVendingMachine : Interactable
{
    [Header("미니게임 설정")]
    [Tooltip("게임 진행 시간 (초)")]
    public float gameDuration = 10f;
    [Tooltip("음료수가 나오는 간격 (초)")]
    public float spawnInterval = 0.6f;
    [Tooltip("미니게임을 할 수 있는 최대 횟수")]
    public int maxUses = 3;
    private int remainingUses; // 남은 횟수

    [Header("오브젝트 설정")]
    [Tooltip("쏟아져 나올 음료수 프리팹 배열 (콜라, 사이다 등)")]
    public GameObject[] drinkPrefabs;
    [Tooltip("음료수가 생성될 위치")]
    public Transform spawnPoint;
    [Tooltip("분출되는 힘의 세기")]
    public float launchForce = 5f;

    [Header("UI 설정")]
    [Tooltip("남은 시간을 표시할 텍스트")]
    public TextMeshProUGUI timerText;

    private bool isGameActive = false;

    void Start()
    {
        remainingUses = maxUses; // 시작할 때 남은 횟수를 최대 횟수로 설정
    }

    // 플레이어가 상호작용하면 이 함수가 호출됨
    public override void Interact(PlayerInteraction player)
    {
        // 게임 중이거나, 남은 횟수가 없거나, 음료수 프리팹이 설정되지 않았다면 실행 안 함
        if (isGameActive || remainingUses <= 0 || drinkPrefabs.Length == 0)
        {
            // 남은 횟수가 없다면 상호작용 텍스트를 변경
            if (remainingUses <= 0)
            {
                interactionPrompt = "더는 나오지 않는다.";
            }
            return;
        }

        remainingUses--; // 횟수 1 차감

        // 플레이어 컨트롤러를 가져와서 미니게임을 시작
        StartCoroutine(StartMiniGame(player.GetComponent<FirstPersonController>()));
    }

    private IEnumerator StartMiniGame(FirstPersonController playerController)
    {
        isGameActive = true;
        timerText.gameObject.SetActive(true);

        float remainingTime = gameDuration;

        while (remainingTime > 0)
        {
            remainingTime -= spawnInterval;
            timerText.text = "남은 시간: " + remainingTime.ToString("F1");

            int randomIndex = Random.Range(0, drinkPrefabs.Length);
            GameObject newDrink = Instantiate(drinkPrefabs[randomIndex], spawnPoint.position, Random.rotation);

            Rigidbody rb = newDrink.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 launchDirection = spawnPoint.up + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.3f, 0.3f));
                rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        // 게임 종료
        timerText.gameObject.SetActive(false);
        isGameActive = false;

        // 게임이 끝난 후, 남은 횟수가 0이면 상호작용 텍스트 업데이트
        if (remainingUses <= 0)
        {
            interactionPrompt = "완전히 고장났다.";
        }
    }
}