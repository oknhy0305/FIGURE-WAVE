using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject explanation;// 설명 패널 오브젝트 참조
    void Start()
    {
        Time.timeScale = 0f;// 시간을 멈추고
        explanation = GameObject.Find("Explanation");//오브젝트 찾고
    }

    public void StartButton()//시작 버튼을 누르면 설명 패널 오브젝트를 끄고 시간을 다시 흐르게 함
    {
        explanation.SetActive(false);
        Time.timeScale = 1f;
    }
}
