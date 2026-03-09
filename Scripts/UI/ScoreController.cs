using UnityEngine;
using TMPro;
public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText; // 점수 텍스트 UI 참조
    public LastScore lastscorescript;// LastScore.cs에 있는 DisplayFinalScore()함수를 불러오기 위함
    public int score = 0; // 점수
    
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        scoreText.text = $"Score: {score}";
        
    }
    public void AddScore(int points)// 점수판 갱신 함수
    {
        score += points;// 점수를 적의 포인트 만큼 추가
        if (lastscorescript != null)
        {
            lastscorescript.DisplayFinalScore(score);//스코어의 값이 최종점수값이 됨
        }
    }
}
