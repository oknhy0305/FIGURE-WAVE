using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText; // 인스펙터에서 TimerText 오브젝트를 드래그 앤 드롭
    private float elapsedTime = 0f;// 경과 시간 확인용
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // 1. 시간 측정
        elapsedTime += Time.deltaTime;
        ApplyFlashEffect(); // 깜빡임 로직

        // 2. UI 업데이트 함수 호출
        UpdateTimerUI();
    }
    void UpdateTimerUI()
    {
        if (timerText == null) return;

        // 분과 초로 계산 (예: 125초 -> 02:05)
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);// 00분
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);// 00초

        // 00:00 포맷으로 문자열 만들기
        timerText.text = string.Format("Elapsed Time: {0:00}:{1:00}", minutes, seconds);
    }
    void ApplyFlashEffect()
    {
        if (elapsedTime >= 180f) // 3분(180초) 이상이면 빨간색 고정
        {
            timerText.color = new Color(1f, 0f, 0f, 1f); // Red (R, G, B, A)
        }
        // 2분(120초) 이상이면 빨간색 깜빡임
        else if (elapsedTime >= 120f)
        {
            float alpha = Mathf.PingPong(Time.time * 5f, 1f); // 5배속으로 깜빡
            timerText.color = new Color(1f, 0.5f, 0f, alpha); // Red (R, G, B, A)
            
        }
        // 1분(60초) 이상이면 노란색 깜빡임
        else if (elapsedTime >= 60f)
        {
            float alpha = Mathf.PingPong(Time.time * 3f, 1f); // 3배속으로 깜빡
            timerText.color = new Color(1f, 0.92f, 0.016f, alpha); // Yellow
            
        }
    }
}
