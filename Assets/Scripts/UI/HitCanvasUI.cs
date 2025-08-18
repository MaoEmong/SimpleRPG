using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 데미지 폰트
public class HitCanvasUI : MonoBehaviour
{
    // 표시할 텍스트
    public Text HitDamageText;
    
    // 초기화
    public void Init(int damage, Vector3 Pos, bool critical)
    {
        if (damage <= 0)
        {
            HitDamageText.text = "MISS!";
			HitDamageText.fontSize = 50;
			transform.position = Pos + new Vector3(0, 1, 0);
			HitDamageText.color = Color.blue;

		}
		else
        {
            HitDamageText.text = damage.ToString();

            HitDamageText.fontSize = 50;
            transform.position = Pos + new Vector3(0, 1, 0);

            // 치명타 시 글자색 빨간색 아니면 검은색
            if (critical)
                HitDamageText.color = Color.red;
            else
                HitDamageText.color = Color.black;
        }

        StartCoroutine(TextAction());
    }

    IEnumerator TextAction()
    {
        // 이동할 높이
        float endMovePosY = 2.8f;

        // 데미지 폰트 유지 시간
        float endTime = 1.2f;
        float curTime = 0.0f;

        // 움직이는 속도
        float MoveSpeed = endMovePosY / endTime;

        // 이동 방향
        Vector3 MoveDir = new Vector3(0, MoveSpeed, 0);

        while(curTime < endTime)
        {
            yield return null;

            curTime += Time.deltaTime;

            transform.position += MoveDir * Time.deltaTime;
            HitDamageText.fontSize++;

        }

        // 액션 끝난 후 풀매니저에 반환
        Managers.Pool.Push(this.gameObject);

    }


}
