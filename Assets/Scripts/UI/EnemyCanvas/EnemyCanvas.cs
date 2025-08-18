using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 몬스터 정보 출력용 캔버스
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

    // 캔버스 위치 조절
    // 몬스터에 직접 넣어놓는 방식이 아닌
    // 매 프레임별 몬스터의 머리위로 이동하는 방식
    // 쿼터뷰 방식의 게임이기에 회전값을 미리 지정해두면 따로 조절할 필요없음
    void SetPosition()
    {
        if (fsm == null)
            return;

        HpBar.fillAmount = fsm.State.Hp / (float)fsm.State.MaxHp;

		transform.position = fsm.transform.position;
        transform.position += AddOffset;

    }


}
