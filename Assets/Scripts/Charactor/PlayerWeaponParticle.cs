using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponParticle : MonoBehaviour
{
	ParticleSystem ps;

	public TrailRenderer trail;

	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
		trail.startColor = Color.white;
	}
	private void LateUpdate()
	{
		SetParticle();
	}
	void SetParticle()
	{
		var main = ps.main;

		switch(Managers.GData.playerinfo.UpgradeWeapon)
		{
			case 0:
				if (ps.isPlaying)
					ps.Stop();
				break;
			case 1:
				main.startColor = Color.magenta;
				trail.startColor = Color.magenta;
				break;
			case 2:
				main.startColor = Color.cyan;
				trail.startColor = Color.cyan;
				break;
			case 3:
				main.startColor = Color.gray;
				trail.startColor = Color.gray;
				break;
			case 4:
				main.startColor = Color.blue;
				trail.startColor = Color.blue;
				break;
			case 5:
				main.startColor = Color.green;
				trail.startColor = Color.green;
				break;
			case 6:
				main.startColor = Color.black;
				trail.startColor = Color.black;
				break;
			case 7:
				main.startColor = Color.red;
				trail.startColor = Color.red;
				break;
			case 8:
				main.startColor = Color.white;
				trail.startColor = Color.white;
				break;

		}

		if(Managers.GData.playerinfo.UpgradeWeapon > 0)
		{
			if (!ps.isPlaying)
			{
				ps.Play();
			}
		}

	}

}
