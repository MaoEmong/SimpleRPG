using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ƼŬ(�̿����� 1ȸ�� ��ƼŬ���� ���� ����)
public class MoveDustParticle : MonoBehaviour
{
	ParticleSystem particle;

	private void Start()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		// ��ƼŬ�� ����� �����ٸ� ������ƮǮ�� ��ȯ
		if(particle.isStopped)
		{
			Managers.Pool.Push(this.gameObject);
		}
	}

}
