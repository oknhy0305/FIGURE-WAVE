using UnityEngine;

public class KeepUpdateUI : MonoBehaviour
{
    // 적 오브젝트로부터 UI가 떨어져 있을 거리
    public Vector3 offset = new Vector3(0, 1.2f, 0); 
    private Transform parentTransform;// 부모 오브젝트 위치 참조

    void Start()
    {
        // 부모인 적의 Transform을 가져옵니다.
        parentTransform = transform.parent;
    }

    void LateUpdate()// 모든 업데이트 함수가 호출 된 후 호출
    {
        if (parentTransform == null) return;

        // 1. 회전을 초기화 (적이 돌아가도 UI는 항상 앞을 봄)
        transform.rotation = Quaternion.identity;

        // 2. 위치를 적의 위치 + 오프셋으로 고정
        transform.position = parentTransform.position + offset;
    }
}