using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ��Ʈ
public class HitCanvasUI : MonoBehaviour
{
    // ǥ���� �ؽ�Ʈ
    public Text HitDamageText;
    
    // �ʱ�ȭ
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

            // ġ��Ÿ �� ���ڻ� ������ �ƴϸ� ������
            if (critical)
                HitDamageText.color = Color.red;
            else
                HitDamageText.color = Color.black;
        }

        StartCoroutine(TextAction());
    }

    IEnumerator TextAction()
    {
        // �̵��� ����
        float endMovePosY = 2.8f;

        // ������ ��Ʈ ���� �ð�
        float endTime = 1.2f;
        float curTime = 0.0f;

        // �����̴� �ӵ�
        float MoveSpeed = endMovePosY / endTime;

        // �̵� ����
        Vector3 MoveDir = new Vector3(0, MoveSpeed, 0);

        while(curTime < endTime)
        {
            yield return null;

            curTime += Time.deltaTime;

            transform.position += MoveDir * Time.deltaTime;
            HitDamageText.fontSize++;

        }

        // �׼� ���� �� Ǯ�Ŵ����� ��ȯ
        Managers.Pool.Push(this.gameObject);

    }


}
