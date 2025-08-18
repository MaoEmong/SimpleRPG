using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ���� ��¿� ĵ����
public class EnemyCanvas : MonoBehaviour
{
    public Text EnemyName;
    public Vector3 AddOffset;
    public Image HpBar;

    EnemyFSM fsm;

    public void Init(EnemyFSM enemy)
    {
        fsm = enemy;
        EnemyName.text = $"Lv.{fsm.State.Level} {fsm.State.Name}";
        HpBar.fillAmount = fsm.State.Hp / fsm.State.MaxHp;
    }

    private void LateUpdate()
    {
        SetPosition();
    }

    // ĵ���� ��ġ ����
    // ���Ϳ� ���� �־���� ����� �ƴ�
    // �� �����Ӻ� ������ �Ӹ����� �̵��ϴ� ���
    // ���ͺ� ����� �����̱⿡ ȸ������ �̸� �����صθ� ���� ������ �ʿ����
    void SetPosition()
    {
        if (fsm == null)
            return;

        HpBar.fillAmount = fsm.State.Hp / (float)fsm.State.MaxHp;

		transform.position = fsm.transform.position;
        transform.position += AddOffset;

    }


}
