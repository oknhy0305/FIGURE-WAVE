using UnityEngine;
using System.Collections;// 코루틴을 쓰기위해 추가한 라이브러리
using UnityEngine.UI;// ui를 사용할 때 추가해야하는 라이브러리
using TMPro;

public class EmemyHealth : MonoBehaviour
{
    [Header("Stats")] // <-헤더로 묶어서 인스펙터에서 깔끔하게 보이게 정리할수있음
    public int maxHealth = 100;   // 최대 체력
    public int currentHealth;    // 현재 체력
    public int score;// 점수

    [Header("Sound Settings")]
    public AudioClip hitSound;//오디오 파일을 넣기 위한 곳
    private AudioSource audioSource;// 오디오소스 컴포넌트를 추가할 예정

    [Header("Image")]
    public Image healthBarFill; // 체력바 이미지 (Fill Amount 조절)
    private SpriteRenderer spriteRenderer;

    public TextMeshProUGUI healthText; // 체력 텍스트 UI 참조
    private Color originalColor;

    private EnemyGenerator generatorScript; // 컴포넌트 가져와서 현재 적 수를 확인 할 예정
    private ScoreController scoreCtrl;// 점수 추가용
    private bool isDead = false; // 중복 사망 방지 플래그
    void Start()
    {
        currentHealth = maxHealth;// 생성될때 현재체력을 최대체력으로 설정 => 최대체력이 늘어나도 그 최대 체력대로 생성
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 원래 색상 저장
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();//컴포넌트 추가
        }

        audioSource.playOnAwake = false; // 생성되자마자 소리 나는 것 방지

        GameObject genObj = GameObject.Find("Generator");
        if (genObj != null) generatorScript = genObj.GetComponent<EnemyGenerator>();
        GameObject scObj = GameObject.Find("ScoreController"); 
        if (scObj != null)
        {
            scoreCtrl = scObj.GetComponent<ScoreController>();
        }
        UpdateHealthBar(); // 초기 체력바 업데이트
    }
    public IEnumerator FlashRed()// 피격당했을때 빨간색으로 깜박거리는 코루틴
    {
        // 1. 빨간색으로 변경
        spriteRenderer.color = Color.red;
        
        // 2. 아주 잠깐 대기 (0.1초)
        yield return new WaitForSeconds(0.1f);
        
        // 3. 원래 색상으로 복구
        spriteRenderer.color = originalColor;
    }

    public void TakeDamage(int damage)//무기에 맞아 피격당행을때 호출되는 함수
    {
        if (isDead) return; // 이미 죽은 상태면 무시

        currentHealth -= damage;//데미지 만큼 체력 까기
        
        // --- 사운드 재생 ---
        if (hitSound != null && audioSource != null)
        {
            //여러 소리가 나와도 부드럽게 들림
            audioSource.PlayOneShot(hitSound, 0.5f);
        }

        StartCoroutine(Knockback(5f, 0.1f));
        StartCoroutine(FlashRed());
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public IEnumerator Knockback(float punch, float duration)// 피격당했을때 밀려나는 코루틴
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;
        float elapsed = 0f;
        Vector2 knockbackDir = (transform.position - player.transform.position).normalized; // 적이 플레이어를 향하는 방향의 반대(뒤쪽)

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            
            // 매 프레임 조금씩 뒤로 이동 (물리 충돌 유지)
            transform.Translate(knockbackDir * punch * Time.deltaTime, Space.World);
            
            yield return null;
        }
    }
    public void Die()//죽음 함수
    {
        if (isDead) return;
        isDead = true;

        // 죽을 때 사운드 재생 <= 이렇게 안하면 맞으면서 죽으면 피격 소리가 안들림
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound, 0.5f);
        }

        // 점수 및 카운트 처리
        if (generatorScript != null) generatorScript.currentCount--;
        if (scoreCtrl != null) scoreCtrl.AddScore(score);

        // 시각적으로만 즉시 제거 (소리는 계속 나오게 함)
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (healthBarFill != null) healthBarFill.transform.parent.gameObject.SetActive(false); // 체력바 부모 오브젝트 비활성화
        
        GetComponent<Collider2D>().enabled = false; // 더 이상 맞지 않게 충돌체 제거

        // 0.5초 뒤에 실제로 오브젝트 파괴 (사운드가 재생될 시간 확보)
        Destroy(gameObject, 0.5f);
    }
    
    void UpdateHealthBar()// 체력바 이미지와 텍스트 업데이트 함수
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

}