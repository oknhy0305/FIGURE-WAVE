using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;// 이동속도
    void Start()
    {
        Application.targetFrameRate = 60;// 목표프레임을 60으로 설정
    }

    void Update()// 이속에 Time.deltaTime을 곱해서 부드러운 움직임을 만들기
    {
        if (Keyboard.current.wKey.isPressed)// W키
        {
            float move = speed * Time.deltaTime;
            transform.Translate(0 , move, 0);
        }
        if (Keyboard.current.sKey.isPressed)// S키
        {
            float move = speed * Time.deltaTime;
            transform.Translate(0 , -move, 0);
        }
        if (Keyboard.current.aKey.isPressed)//A키
        {
            float move = speed * Time.deltaTime;
            transform.Translate(-move, 0, 0);
        }
        if (Keyboard.current.dKey.isPressed)//D키
        {
            float move = speed * Time.deltaTime;
            transform.Translate(move, 0, 0);
        }
        
    }
}

