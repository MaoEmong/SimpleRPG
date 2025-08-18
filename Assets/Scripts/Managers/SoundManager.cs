using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드 매니저
public class SoundManager
{
    // 미리 지정한 크기의 오디오리소스 생성
	AudioSource[] _audioSource = new AudioSource[(int)Define.Sound.MaxCount];
    // 1회 이상 재생한 오디오 데이터 저장용
	Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip> ();

    // 초기화
	public void Init()
	{
		GameObject root = GameObject.Find("Sound");
		if(root == null)
		{
            root = new GameObject { name = "Sound" };
			Object.DontDestroyOnLoad(root);

			string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
			for(int i = 0; i < soundNames.Length - 1; i++)
			{
				GameObject sound = new GameObject { name = soundNames[i] };
				_audioSource[i] = sound.AddComponent<AudioSource>();
				sound.transform.parent = root.transform;
			}
		}
        // Bgm은 반복설정
		_audioSource[(int)Define.Sound.Bgm].loop = true;
	}
    // 저장중인 오디오 데이터 날림
	public void Clear()
	{
		foreach(var audio in _audioSource)
		{
			audio.clip = null;
			audio.Stop();
		}
		_audioClips.Clear();
	}

    // Resources폴더의 경로안에 있는 파일 재생
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        // 설정한 오디오가 오디오클립에 존재하는지 확인 후 가져오기
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        // 선택한 음원이 Bgm이라면 
        if (type == Define.Sound.Bgm)
        {
            // 현재 재생중인 음원 멈추고
            AudioSource audioSource = _audioSource[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            // 새로 재생
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        // 선택한 음원이 Effect라면
        else
        {
            // 바로 재생(1회)
            AudioSource audioSource = _audioSource[(int)Define.Sound.Effect];
            // 피치값을 랜덤하게 주어 중복된 이펙트 재생시 공명현상 미리 차단
            audioSource.pitch = Random.Range(pitch*0.9f, pitch);
            audioSource.PlayOneShot(audioClip);
        }
    }

    // 경로로 입력한 위치에 있는 오디오 클립 찾아
    // type으로 설정한 오디오소스에 넣어둠
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            // Bgm의 경우 여러번 재생되는게 아니기에 필요시 리소스폴더에서 찾아옴
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            // Effect의 경우 여러번 재생되기에 1회 이상 재생된 오디오클립은 미리 저장해 두었다가 가져다씀
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                // 재생된 적이 없다면 리소스폴더에서 찾아 가져옴
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void BgmStop()
    {
		// 현재 재생중인 bgm이 있다면 멈춤
		AudioSource audioSource = _audioSource[(int)Define.Sound.Bgm];
		if (audioSource.isPlaying)
			audioSource.Stop();
	}

	// Bgm과 Effect의 음량 조절
	public void SetBGMVol(float vol)
    {
        _audioSource[(int)Define.Sound.Bgm].volume = vol;
    }
    public float GetBGMVol()
    {
        return _audioSource[(int)Define.Sound.Bgm].volume;
    }
    public void SetEffectVol(float vol) 
    {
        _audioSource[(int)Define.Sound.Effect].volume = vol * 0.8f;
    }
    public float GetEffectVol()
    {
        return _audioSource[(int)Define.Sound.Effect].volume;
    }

}
