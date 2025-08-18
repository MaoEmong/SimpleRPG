using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MyDirector : MonoBehaviour
{
    public Image CinemachineImage;
    public PlayableDirector SceneDirector;
    public GameObject CinemachineCameras;
    public Image BlackImage;
    void Start()
    {
        CinemachineImage.gameObject.SetActive(false);

        SceneDirector = GetComponent<PlayableDirector>();

        StartCoroutine(StartCinemachine());
    }

    IEnumerator StartCinemachine()
    {
        CinemachineImage.gameObject.SetActive(true);

        SceneDirector.Play();

        while(true)
        {
            yield return null;

            if(SceneDirector.time >= SceneDirector.duration)
            {
                Debug.Log("Finish Action");

				StartCoroutine(MyTools.ImageFadeOut(CinemachineImage, 0.5f));
				Managers.CallWaitForSeconds(0.5f, () => { CinemachineImage.gameObject.SetActive(false); });
				
                BlackImage.gameObject.SetActive(true);
				StartCoroutine(MyTools.ImageFadeOut(BlackImage, 0.5f));
				Managers.CallWaitForSeconds(0.5f, () => { BlackImage.gameObject.SetActive(false); });

				CinemachineCameras.SetActive(false);
				gameObject.SetActive(false);

				break;
            }

        }


    }

}
