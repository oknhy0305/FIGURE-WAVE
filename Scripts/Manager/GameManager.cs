using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가

public class GameManager : MonoBehaviour
{
    // 버튼 클릭 시 호출할 함수
    public void RestartGame()// 게임 재시작 버튼에 연결 할 함수
    {
        //게임 재시작 버튼을 눌렀을때 Time.timeScale을 1f로 만들어야 시간이 정상적으로 흐름,멈췄던 시간을 다시 시작
        Time.timeScale = 1f;
        
        // 현재 활성화 된 씬의 이름을 가져와서 다시 로드 => 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}