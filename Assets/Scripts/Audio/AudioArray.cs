using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioArray : MonoBehaviour
{
	public AudioSource audioSource
	{
		get
		{
			if (_audioSource == null)
			{
				_audioSource = this.GetComponent<AudioSource>();
			}
			return _audioSource;
		}
	}
	private AudioSource _audioSource;

	[Tooltip("AudioClips used for either PlayAtIndex() or PlayRandom().")]
	[SerializeField]
	private AudioClip[] AudioPool;

	/// <summary> play a random clip from <seealso cref="AudioPool"/>.</summary>
	[ContextMenu("PlayRandom")]
	public void PlayRandom()
	{
		PlayRandom(false);
	}

	[ContextMenu("PlayRandomOneShot")]
	public void PlayRandomOneShot()
	{
		PlayRandom(true);
	}

	public void PlayRandom(bool oneShot = false)
	{
		int atIndex = Random.Range(0, AudioPool.Length);
		Play(atIndex, oneShot);
	}

	/// <summary> play a clip from <seealso cref="AudioPool"/> at <paramref name="atIndex"/>. </summary>
	public void Play(int atIndex, bool oneShot = false)
	{
		var clipToPlay = AudioPool[atIndex];
		if (clipToPlay)
		{
			if (oneShot)
			{
				audioSource.PlayOneShot(AudioPool[atIndex]);
			}
			else if (audioSource.clip != clipToPlay)
			{
				audioSource.clip = clipToPlay;
				audioSource.Play();
			}
		}
		else if (false == oneShot)
		{
			audioSource.clip = null;
		}
	}
}
