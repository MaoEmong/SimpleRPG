using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ī�޶� �̵� ��ũ��Ʈ
public class CameraMovement : MonoBehaviour
{
	// ī�޶� ����ٴ� Ÿ��(�÷��̾�)
	public GameObject Target;
	// ī�޶��� ���� ��ġ��
	public Vector3 TargetPlayerCameraPos;
	// ī�޶��� ���� ȸ����
	public Quaternion TargetPlayerCameraRot;
	// ī�޶� �÷��̾ �ٶ󺼰����� üũ
	public bool isLookPlayer = true;
	// �÷��̾ ������ ������Ʈ�� ��Ȱ��ȭ/Ȱ��ȭ üũ�� �ڷ�ƾ
	Coroutine Corou = null;

	public bool isshake = false;

	private void LateUpdate()
	{
		// �÷��̾ ��Ȱ��ȭ ���¶�� return
		if (!Target.activeSelf)
			return;
		// �÷��̾ �ٶ����ʰڴٸ� return
		if (!isLookPlayer)
			return;

		// �÷��̾��� ��ġ�� �̵� �� ������ġ���� ����ȸ������ �����Ѵ�
		transform.position = Target.transform.position;
		transform.position += TargetPlayerCameraPos;
		transform.rotation = TargetPlayerCameraRot;
		//�÷��̾�������� ����ĳ��Ʈ �߻�
		CheckLay();
		CameraShakeAction();
	}

	// ī�޶� ����
	void CameraShakeAction()
	{
		if (!isshake)
			return;
		// ī�޶� ���Ⱑ Ȱ��ȭ �Ǹ�
		else
		{
			// �� �����Ӻ� ������ �����ǰ��� �������� ���� ���
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

		// ī�޶󿡼� �÷��̾�� ����ĳ��Ʈ �߻�
		if(Physics.Raycast(StartRayPos, Dir,out hit))
		{
			// �÷��̾�� ī�޶� ���̿� �ǹ� ������Ʈ�� ��������
			if(hit.collider.gameObject.CompareTag("Buildings"))
			{
				Debug.Log($"Hit! / {hit.collider.name}");
				GameObject hitobj = hit.collider.gameObject;
				// �ش� �ǹ��� Ȱ��ȭ�� ���¶��
				if (hitobj.GetComponent<MeshRenderer>().enabled)
					// ��Ȱ��ȭ��Ŵ
					hitobj.GetComponent<MeshRenderer>().enabled = false;
				// �ڷ�ƾ�� ������� �ʴٸ�
				if(Corou != null)
				{
					// �ڷ�ƾ�� ����
					StopCoroutine(Corou);
				}
				// �ǹ��� ��Ȱ��ȭ��Ű�� �ڷ�ƾ ����
				Corou = StartCoroutine(ActiveTrue(hit.collider.gameObject));
			}
		}

	}

	IEnumerator ActiveTrue(GameObject obj)
	{
		// ���� �ð� �� �ش� ������Ʈ�� Ȱ��ȭ
		yield return new WaitForSeconds(0.3f);
		obj.GetComponent<MeshRenderer>().enabled = true;
	}


}
