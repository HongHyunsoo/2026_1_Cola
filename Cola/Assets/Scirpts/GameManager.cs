using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 사용하므로 UI.Text가 아님을 확인하세요.

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("게임 시간 설정")]
    [Tooltip("게임 시작 시각 (24시간 기준)")]
    public int startTimeInHours = 4;
    [Tooltip("총 게임 진행 시간 (게임 시각 기준)")]
    public int gameDurationInHours = 12;

    [Header("UI 설정")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI warningText; // 경고 문구를 표시할 텍스트

    // 내부 변수
    private float totalRealTimeSeconds; // 총 실제 게임 시간(초)
    private float currentTime; // 현재 남은 실제 시간(초)
    private bool warningShown = false; // 경고가 한 번만 표시되도록 하는 플래그

    [Header("연료(콜라) 설정")]
    public float maxFuel = 100f; // 최대량은 나중을 위해 남겨둬도 좋습니다.
    public float currentFuel = 0f;
    // ▼▼▼ 이 부분을 Image에서 TextMeshProUGUI로 변경합니다 ▼▼▼
    public TextMeshProUGUI fuelText; // 기존: public Image fuelGaugeImage;
    // 연료 게이지 UI는 PlayerInteraction이 아닌 GameManager가 직접 관리해야 합니다.
    // public Image fuelGaugeImage; <- 이 부분은 PlayerInteraction에서 삭제하고 여기에 연결해야 합니다.

    [Header("엔딩 설정")]
    private bool isPlayerInSafeArea = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시간 1분 = 현실 시간 1초
        // 총 게임 시간(12시간) = 12 * 60 = 720 게임 분 = 720 현실 초
        totalRealTimeSeconds = gameDurationInHours * 60f;
        currentTime = totalRealTimeSeconds;

        if (warningText != null)
        {
            warningText.gameObject.SetActive(false); // 시작할 때 경고 문구 숨기기
        }

        UpdateFuelUI(); // 게임 시작 시 연료 UI도 초기화
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // 현실 시간 60초 남았을 때 경고 문구 표시
            if (currentTime <= 60f && !warningShown)
            {
                ShowWarning();
                warningShown = true;
            }
        }
        else
        {
            currentTime = 0;
            EndGame();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // 지나간 현실 시간(초) = 지나간 게임 시간(분)
            float elapsedGameMinutes = totalRealTimeSeconds - currentTime;

            // 현재 게임 시각 계산
            float totalMinutesFromStart = (startTimeInHours * 60) + elapsedGameMinutes;
            int currentHour = Mathf.FloorToInt(totalMinutesFromStart / 60);
            int currentMinute = Mathf.FloorToInt(totalMinutesFromStart % 60);

            timerText.text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);
        }
    }

    void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            warningText.text = "로켓 발사 1분 전! 목표 지점으로 이동하세요!";
            // 여기에 추가로 사운드를 재생하거나, 텍스트를 깜빡이는 효과를 넣을 수도 있습니다.
        }
    }

    private void EndGame()
    {
        this.enabled = false;
        if (isPlayerInSafeArea)
        {
            SceneManager.LoadScene("EndingScene");
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void SetPlayerSafeAreaStatus(bool status)
    {
        isPlayerInSafeArea = status;
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateFuelUI(); // 이름이 바뀐 함수를 호출합니다.
    }

    void UpdateFuelUI()
    {
        if (fuelText != null)
        {
            // 소수점 둘째 자리까지 표시하고 뒤에 " L"를 붙입니다.
            fuelText.text = currentFuel.ToString("F2") + " L";
        }
    }
}
