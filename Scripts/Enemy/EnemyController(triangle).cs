//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;// 플레이어 위치 담기
    public float speed = 3f;//이속
    float rotationOffset = 90f;
    //유니티에서는 0도를 기준으로 x좌표를 늘리는 방식으로 한 대상을 추적하는 움직임을 만들수있는데
    //이미지 스프라이트의 앞부분이 아래로 향하고 있어서 오프셋을 설정했음 
    Rigidbody2D rb;
    public int damage; // 플레이어에게 입힐 데미지
    GameObject genrator; // Generator 스크립트가 붙은 게임 오브젝트 참조
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;//플레이어 태그로 찾기
        }
        genrator = GameObject.Find("Generator"); // Generator 오브젝트 찾기
    }
    private void FixedUpdate() //일정한 주기로 업데이트
    {
        Vector2 direction = (player.position - transform.position).normalized;//목적지와 출발지를 빼서 방향(거리포함)을 구하고 거리를 1로 바꿔서 플레이어로 향하는 방향을 구함
        float angle = Mathf.Atan2(direction.y, direction.x) *  Mathf.Rad2Deg;//구한 방향을 각도로 변한 시킨다고 하는데 이해할려 했지만 무슨 말인지 모르겠음(나중에 더 검색해보기) 
        rb.MoveRotation(angle + rotationOffset);// 부드럽게 회전시키기 + 회전 오프셋을 더해서 방향 정리
        rb.linearVelocity = direction * speed;// 방향으로 움직이기, linearVelocity는 물리적인 상호작용이 좋아서 사용
    }
    private void OnCollisionEnter2D(Collision2D collision)//적이 플레이어한테 부딪히고 데미지를 준 후 적이 없어지는 함수 
    {
        if (collision.gameObject.CompareTag("Player"))//충돌한것이 플레이어인지 태그로 확인
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();//플레이어 체력 가져오기
            // 2. 컴포넌트가 제대로 있다면 데미지를 입힘
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // 데미지 10을 줌
            }
            GetComponent<Collider2D>().enabled = false;// 히트박스 끄기
            Destroy(gameObject);// 적 (지금 오브젝트) 파괴
            genrator.GetComponent<EnemyGenerator>().currentCount--; // Generator 스크립트의 currentCount 감소
        }
    }
}