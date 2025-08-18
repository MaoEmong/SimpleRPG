using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFade : MonoBehaviour
{
	MeshRenderer mesh = null;


	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Buildings"))
		{
			if(mesh == null)
				mesh = other.gameObject.GetComponent<MeshRenderer>();
		}

		OnTriggerStay(other);
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Buildings"))
		{
			if (mesh != null)
				mesh.enabled = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Buildings"))
		{
			if(mesh != null)
				mesh.enabled = true;
		}
	}

}
