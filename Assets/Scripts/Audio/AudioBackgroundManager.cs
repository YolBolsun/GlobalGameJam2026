using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioBackgroundManager : MonoBehaviour
{
	public enum MusicScenes
	{
		None,
		Fight,
		NightClub,
		Ripperdoc,
	}

	public static AudioBackgroundManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindFirstObjectByType<AudioBackgroundManager>();
			}
			return instance;
		}
	}
	private static AudioBackgroundManager instance;

	/// <summary>
	/// This property enables "Faint club music (gets distorted as the game plays out)". It fades between two audio mixer snapshots using https://www.youtube.com/watch?v=2nYyws0qJOM
	/// </summary>
	public float DistortionPercent
	{
		get
		{
			return distortionPercent;
		}
		set
		{
			OnValidate();
			distortionPercent = value;
		}
	}

	[Header("Stuff to modify")]
	[Tooltip("How often the distortion feedback is audible. 1s to 300s.")]
	[Range(0, 1)]
	[SerializeField]
	private float distortionPercent = 0.1f;

	[Tooltip("Type of music to play in the background.")]
	[SerializeField]
	private MusicScenes musicType = MusicScenes.Fight;

	[Header("Stuff to NOT modify")]
	[SerializeField]
	private AudioMixerSnapshot undistorted;
	[SerializeField]
	private AudioMixerSnapshot distorted;
	[SerializeField]
	private AudioArray music;

	/// <summary> true when <seealso cref="distortionCoroutine"/> is running. </summary>
	private bool pendingDistortion;

	private Coroutine distortionCoroutine;

	void Update()
	{
		if (distortionPercent <= 0.01 || pendingDistortion)
		{
			return;
		}

		distortionCoroutine = StartCoroutine(playDistortion());
	}

	public void ChangeMusic(MusicScenes toMusic)
	{
		if (music)
		{
			music.Play((int)toMusic);
		}
	}

	/// <summary> allows changes to distortionPercent to be tested in the Unity Editor </summary>
	private void OnValidate()
	{
		if (false == Application.isPlaying)
		{
			return;
		}

		if (distortionCoroutine != null) // null only if update hasn't run yet
		{
			StopCoroutine(distortionCoroutine);
			undistorted.TransitionTo(0.1f);
			pendingDistortion = false;
		}

		ChangeMusic(musicType);
	}

	private IEnumerator playDistortion()
	{
		// play normal background
		pendingDistortion = true;
		float secondsNormal = Mathf.Lerp(300, 1, distortionPercent);
		// Debug.Log("AudioBackgroundManager Normal for seconds:" + secondsNormal);
		yield return new WaitForSeconds(secondsNormal);

		// emphasize distortion in background audio
		distorted.TransitionTo(0.1f);
		yield return new WaitForSeconds(2.9f);

		// return to play normal background
		undistorted.TransitionTo(0.1f);
		pendingDistortion = false;
	}
}
