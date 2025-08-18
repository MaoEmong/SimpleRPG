using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직임 파티클(이였으나 1회성 파티클에도 적용 가능)
public class MoveDustParticle : MonoBehaviour
{
	ParticleSystem particle;

	private void Start()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		// 파티클의 재생이 끝난다면 오브젝트풀로 반환
		if(particle.isStopped)
		{
			Managers.Pool.Push(this.gameObject);
		}
	}

}
