using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 카메라 이동 스크립트
public class CameraMovement : MonoBehaviour
{
	// 카메라가 따라다닐 타겟(플레이어)
	public GameObject Target;
	// 카메라의 고정 위치값
	public Vector3 TargetPlayerCameraPos;
	// 카메라의 고정 회전값
	public Quaternion TargetPlayerCameraRot;
	// 카메라가 플레이어를 바라볼것인지 체크
	public bool isLookPlayer = true;
	// 플레이어를 가리는 오브젝트의 비활성화/활성화 체크용 코루틴
	Coroutine Corou = null;

	public bool isshake = false;

	private void LateUpdate()
	{
		// 플레이어가 비활성화 상태라면 return
		if (!Target.activeSelf)
			return;
		// 플레이어를 바라보지않겠다면 return
		if (!isLookPlayer)
			return;

		// 플레이어의 위치로 이동 후 고정위치값과 고정회전값을 설정한다
		transform.position = Target.transform.position;
		transform.position += TargetPlayerCameraPos;
		transform.rotation = TargetPlayerCameraRot;
		//플레이어방향으로 레이캐스트 발사
		CheckLay();
		CameraShakeAction();
	}

	// 카메라 흔들기
	void CameraShakeAction()
	{
		if (!isshake)
			return;
		// 카메라 흔들기가 활성화 되면
		else
		{
			// 매 프레임별 각각의 포지션값에 난수값을 더해 흔듦
			float RandX = Random.Range(-0.3f, 0.3f);
			float RandY = Random.Range(-0.3f, 0.3f);
			float RandZ = Random.Range(-0.3f, 0.3f);

			Vector3 shakePos = new Vector3(RandX, RandY, RandZ);

			transform.position += shakePos;
		}
	}

	void CheckLay()
	{
		RaycastHit hit;

		Vector3 Dir = Target.transform.position - transform.position;
		Vector3 DirReverse = transform.position - Target.transform.position;
		Vector3 DirReverseNormal = DirReverse.normalized;
		Vector3 StartRayPos = transform.position + DirReverseNormal * 10.0f;

		// 카메라에서 플레이어로 레이캐스트 발사
		if(Physics.Raycast(StartRayPos, Dir,out hit))
		{
			// 플레이어와 카메라 사이에 건물 오브젝트가 들어왔을떄
			if(hit.collider.gameObject.CompareTag("Buildings"))
			{
				Debug.Log($"Hit! / {hit.collider.name}");
				GameObject hitobj = hit.collider.gameObject;
				// 해당 건물이 활성화된 상태라면
				if (hitobj.GetComponent<MeshRenderer>().enabled)
					// 비활성화시킴
					hitobj.GetComponent<MeshRenderer>().enabled = false;
				// 코루틴이 비어있지 않다면
				if(Corou != null)
				{
					// 코루틴을 중지
					StopCoroutine(Corou);
				}
				// 건물을 비활성화시키는 코루틴 시작
				Corou = StartCoroutine(ActiveTrue(hit.collider.gameObject));
			}
		}

	}

	IEnumerator ActiveTrue(GameObject obj)
	{
		// 일정 시간 후 해당 오브젝트를 활성화
		yield return new WaitForSeconds(0.3f);
		obj.GetComponent<MeshRenderer>().enabled = true;
	}


}
