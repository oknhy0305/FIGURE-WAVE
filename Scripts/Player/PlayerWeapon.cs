using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    public Transform playertransform;// 플레이어 위치로 무기를 불러오기 위해 위치 참조
    public float swingDuration = 0.2f;  // 휘두르는 시간
    public Vector3 storagePosition = new Vector3(100, 100, 0); // 평소 무기 대기 장소, 멀리 두어서 게임 화면에 안보이게 + 충돌 오류 방지
    private bool isSwinging = false;// 현재 휘두르는 중인지
    private SpriteRenderer spriteRenderer;
    float time = 0;
    public int damage; // 무기가 입히는 데미지 양
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 시작할 때 무기를 대기 장소로 보냄
        transform.position = storagePosition;
        SetAlpha(0f);// 투명
    }   

    void Update()
    {
        this.time += Time.deltaTime;
        if (this.time > 1f && !isSwinging)
        {
            StartCoroutine(SwingRoutine());//휘두르는 코루틴 실행
            this.time = 0;
        }
    }
    IEnumerator SwingRoutine()// 휘두르는 코루틴
    {
        isSwinging = true;
        SetAlpha(1f);// 투명에서 보이게

        float elapsed = 0f;
        
        // 회전값 초기화
        Quaternion startRotation = Quaternion.identity;

        while (elapsed < swingDuration)// 회전이 지속 되는 시간 반복
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;

            // 1. 위치 동기화: 휘두르는 동안에도 플레이어를 따라다님
            transform.position = playertransform.position;

            // 2. 회전: 0도에서 360도까지 t 비율에 따라 회전
            float zRotation = Mathf.Lerp(0, -360f, t);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);

            yield return null;
        }

        // 3. 휘두르기 종료 후 처리
        SetAlpha(0f);//투명
        transform.position = storagePosition; // 원래 위치(대기 장소)로 복귀
        isSwinging = false;
    }

    void SetAlpha(float alpha)// 색 변환 함수
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)// 무기와 적 충돌 확인 함수
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EmemyHealth enemyHealth = collision.gameObject.GetComponent<EmemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // 데미지 20을 줌
            }
        }   
    }
    public void del()// 파괴 함수
    {
        Destroy(gameObject);
    }
}