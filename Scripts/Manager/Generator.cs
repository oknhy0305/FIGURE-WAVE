using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject triangleEnemyPrefab; //삼각형 적 프리팹 연결
    public GameObject squareEnemyPrefab; //사각형
    public GameObject pentagonEnemyPrefab; //오각형
    public float spawnInterval = 2.0f; // 생성 간격 (1f가 1초임)
    public float spawnRadius = 10f; // 플레이어로부터 떨어진 생성 거리
    public LayerMask wallLayer; // 인스펙터에서 'Wall' 레이어를 선택해야 함
    public float checkRadius = 0.5f; // 적의 크기에 맞춘 충돌 체크 범위
    public int currentCount = 0; // 현재 생성된 적의 수
    private float elapsedTime = 0f; // 게임 흐름 시간

    private Transform player;// 플레이어 위치 참조

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 일정 시간마다 SpawnEnemy 함수를 반복 실행 => 집가서 정리////////////
        //============================================================
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;//총 경과시간 기록 
    }

    void SpawnEnemy()
    {
        if (player == null) return;
        if (currentCount >= 50) return; // 최대 50마리까지만 생성
        
        GameObject prefabToSpawn = triangleEnemyPrefab;//생성될 프리팹을 삼각형으로 정함

        if (elapsedTime >= 240f)// 4분 경과
        {
            float roll = Random.value;
            if (roll > 0.5f) // 50% 확률로 오각형
            {
                prefabToSpawn = pentagonEnemyPrefab;
            }
            else if (roll > 0.1f) // 30% 확률로 사각형
            {
                prefabToSpawn = squareEnemyPrefab;
            }
            // 나머지는 일반 적
        }
        if (elapsedTime >= 180f)//3분 경과
        {
            float roll = Random.value;
            if (roll > 0.7f) // 30% 확률로 오각형
            {
                prefabToSpawn = pentagonEnemyPrefab;
            }
            else if (roll > 0.3f) // 40% 확률로 사각형
            {
                prefabToSpawn = squareEnemyPrefab;
            }
            // 나머지는 일반 적
        }
        if (elapsedTime >= 120f) // 2분 경과
        {
            float roll = Random.value;
            if (roll > 0.9f) // 10% 확률로 오각형
            {
                prefabToSpawn = pentagonEnemyPrefab;
            }
            else if (roll > 0.6f) // 30% 확률로 사각형
            {
                prefabToSpawn = squareEnemyPrefab;
            }
            // 나머지는 일반 적
        }
        else if (elapsedTime >= 60f) // 1분 경과
        {
            if (Random.value > 0.7f) // 30% 확률로 사각형
            {
                prefabToSpawn = squareEnemyPrefab;
            }
        } 
        Vector3 spawnPos = GetSafeSpawnPosition();
        if (spawnPos != Vector3.zero)
        {
            GameObject newEnemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);// 적 생성 Instantiate 함수는 여기서는 프리팹,생성위치,회전을 가진다
             //3분이 지났는지 체크 (180초)
            if (elapsedTime >= 180f)
            {
                EmemyHealth healthScript = newEnemy.GetComponent<EmemyHealth>();//체력 가져오기
                EnemyController enemymove = newEnemy.GetComponent<EnemyController>();//이속 가져오기
                
                if (healthScript != null)
                {
                    // 4. 스텟을 2배로 올립니다.
                    healthScript.maxHealth *= (int)elapsedTime / 90; // 최대 체력 2배 + 점진적 증가
                    enemymove.speed += 1;// 이속 + 1
                }
            }
            currentCount++;//적 카운트 올리기
        }
    }
    Vector3 GetSafeSpawnPosition() // 적이 스폰하기에 안전한 위치를 반환하는 함수(세계의 벽 안쪽)
    {
        for (int i = 0; i < 10; i++)// 최대 10번 시도
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;//스폰원을 기준으로=> 집가서 정리////////////////////////////////////// 
            Vector3 pos = new Vector3(randomCircle.x, randomCircle.y, 0) + player.position;// 플레이어 기준 랜덤으로 생성된 좌표를 더하여 적을 생성할 위치를 정함

            if (Physics2D.OverlapCircle(pos, 0.5f, wallLayer) == null) // OverlapCircle 함수는 지정한 위치에 원을그리고 그 원이 벽과 겹치는지 체크. 겹치지 않으면 null 반환
                return pos;// 벽과 겹치지 않는 위치를 찾으면 그 위치 반환
        }
        return Vector3.zero;
    }
}