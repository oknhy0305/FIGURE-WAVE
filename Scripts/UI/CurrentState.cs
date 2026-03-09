using UnityEngine;
using TMPro;
public class CurrentState : MonoBehaviour
{
    private TextMeshProUGUI currentstate; //텍스트 ui 참조
    private float elapsedTime = 0f;

    void Start()
    {
        currentstate = GetComponent<TextMeshProUGUI>();//텍스트에 접근하기 위해 컴포넌트 가져오기
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 180f) // 3분(180초) 이상이면 빨간색 고정 + 더 강한 적
        {
            currentstate.text = "<color=#DC143C>STRONGER ENEMY</color>";//Rich Text Tag라고 불리우는 TMPro 라이브러리의 기능,html처럼 태그로 속성을 바꿀 수 있다.
        }
        else if (elapsedTime >= 120f)//삼각형+사각형+오각형 적 텍스트
        {
            currentstate.text = "Generate Triangle Enemy\n<color=#FFFF00>+ Generate Square Enemy</color>\n<color=#FFA500>+ Generate Pentagon Enemy</color>";
        }
        else if (elapsedTime >= 60f)//삼각형+사각형 적 텍스트
        {
            currentstate.text = "Generate Triangle Enemy\n<color=#FFFF00>+ Generate Square Enemy</color>";
        }
        else if (elapsedTime >= 0f)// 삼각형 적 생성 텍스트
        {
            currentstate.text = "Generate Triangle Enemy";
        }
    }
}
