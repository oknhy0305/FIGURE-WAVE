using UnityEngine;
using TMPro;

public class LastScore : MonoBehaviour
{
    private TextMeshProUGUI lastScoreText;

    void Awake()//start함수보다 먼저 호출
    {
        lastScoreText = GetComponent<TextMeshProUGUI>();
    }

    // ScoreController에서 호출할 함수, 최종점수 계산
    public void DisplayFinalScore(int currentScore)
    {
        // 텍스트 컴포넌트가 아직 할당 안 되었을 경우를 대비해 다시 확인
        if (lastScoreText == null) lastScoreText = GetComponent<TextMeshProUGUI>();
        
        if (lastScoreText != null)
        {
            lastScoreText.text = $"Your score: {currentScore}";// ScoreController에서 점수가 계속 더해지는 방식으로 계산하는데 그때 더해진 점수를 여기다가 매개변수로 대입함
        }
    }
}