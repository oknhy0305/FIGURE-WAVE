using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;   // 최대 체력
    public int currentHealth;    // 현재 체력

    [Header("Sound")]
    public AudioClip hitSound;//히트 사운드 오디오 파일을 연결
    public AudioClip GameOverSound;//게임 오버 사운드 오디오 파일을 연결
    private AudioSource audioSource;//

    GameObject weapon;

    [Header("UI Reference")]
    public TextMeshProUGUI playerhealthText; // 체력 텍스트 UI 참조
    public GameObject gameOverUI;           // 게임 오버 시 활성화할 패널(Panel) 오브젝트
    public GameObject hud;//HUD ui 참조

    private Color originalHealthText;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        // 시작할 때 체력을 가득 채움
        currentHealth = maxHealth;
        originalHealthText = playerhealthText.color;// 텍스트의 원래 색상 저장
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 원래 색상 저장

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 코드로 직접 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false; // 생성되자마자 소리 나는 것 방지

        weapon = GameObject.Find("circleWeapon_0");// 무기 오브젝트 찾기

        playerhealthText.text = $"Health:\n {currentHealth} / {maxHealth}"; // 초기 체력바 업데이트
        // 게임 시작 시에는 게임 오버 UI를 비활성화
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }
    public IEnumerator FlashRed()// 피격시 빨간색으로 깜빡이는 코루틴
    {
        // 1. 빨간색으로 변경
        spriteRenderer.color = Color.red;
        
        // 2. 아주 잠깐 대기 (0.1초)
        yield return new WaitForSeconds(0.1f);
        
        // 3. 원래 색상으로 복구
        spriteRenderer.color = originalColor;
    }   

    // 체력을 깎는 함수 (외부에서 호출할 예정)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (hitSound != null && audioSource != null)
        {
            // PlayOneShot은 여러 소리가 겹쳐도 부드럽게 들림
            audioSource.PlayOneShot(hitSound, 0.5f);
        }
        StartCoroutine(UpdateHealthText()); // 초기 체력바 업데이트
        //플레이어 빨간색 깜빡임 실행
        StartCoroutine(FlashRed());
        // 체력이 0 이하가 되면 사망 처리
        if (currentHealth <= 0)
        {
            Die();
            weapon.GetComponent<PlayerWeapon>().del();// 무기도 부수기
        }
    }

    void Die()// 죽음 함수
    {
        // 1. 모든 적 오브젝트 파괴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");// 적 오브젝트 찾기
        foreach (GameObject enemy in enemies)// 배열에 적 오브젝트 담고 배열의 길이만큼 반복
        {
            Destroy(enemy);//적 파괴
        }
        // 2. 배경 음악 정지
        GameObject.Find("SoundManager").GetComponent<AudioSource>().Stop();
        // 3. 시간 정지 
        Time.timeScale = 0f;

        // 4. 무기 제거 로직
        if (weapon != null)
        {
            var weaponScript = weapon.GetComponent<PlayerWeapon>();//스크립트 가져오기
            if (weaponScript != null) weaponScript.del();//무기 제거 함수
        }
        // 5. 게임 오버 UI 활성화
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);//비활성화 되있던 게임오버ui키기
            hud.SetActive(false);//게임도중 나오는 ui 비활성화
        }
        // 6. 플레이어 비활성화
        gameObject.SetActive(false);
        if (GameOverSound != null)
        {
            // 플레이어가 사라져도 소리가 들리도록 카메라 위치에서 재생
            AudioSource.PlayClipAtPoint(GameOverSound, Camera.main.transform.position, 1.0f);// 게임 오버 사운드
        }
    }
    public IEnumerator UpdateHealthText()// 체력바 업데이트
    {
        if (playerhealthText != null)
        {
            playerhealthText.text = $"Health:\n {currentHealth} / {maxHealth}";
            playerhealthText.color = new Color(1f, 0, 0, 1f);//색 바꾸고
            yield return new WaitForSeconds(0.1f);//기다리고
            playerhealthText.color = originalColor;//원래 색 => 빨간색으로 깜박임
        }
    }
}
