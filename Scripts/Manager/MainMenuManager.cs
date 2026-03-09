using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Name to Load")]
    public string gameSceneName = "GameSecne"; // 실제 게임이 진행되는 씬 이름

    // '게임 시작' 버튼에 연결할 함수
    public void StartGame()
    {
        // 시간을 정상 속도로 설정 (혹시 이전 게임에서 멈췄을 수 있음)
        Time.timeScale = 1f;
        
        // 지정된 이름의 씬을 로드
        SceneManager.LoadScene(gameSceneName);
    }

    // '게임 종료' 버튼에 연결할 함수
    public void QuitGame()
    {
        // 빌드된 게임 환경에서 응용 프로그램을 종료
        Application.Quit();
        
        // 에디터 상에서는 작동하지 않으므로 확인용 로그 출력
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}