using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioBackgroundManager : MonoBehaviour
{
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

	[Tooltip("How often the distortion feedback is audible. 1s to 300s")]
	[Range(0, 1)]
	[SerializeField]
	private float distortionPercent = 0.1f;

	[SerializeField]
	private AudioMixerSnapshot undistorted;
	[SerializeField]
	private AudioMixerSnapshot distorted;

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

	/// <summary> allows changes to distortionPercent to be tested in the Unity Editor </summary>
	private void OnValidate()
	{
		if (distortionCoroutine != null)
		{
			StopCoroutine(distortionCoroutine);
			pendingDistortion = false;
		}
	}

	private IEnumerator playDistortion()
	{
		// play normal background
		pendingDistortion = true;
		float secondsNormal = Mathf.Lerp(300, 1, distortionPercent);
		Debug.Log("AudioBackgroundManager Normal for seconds:" + secondsNormal);
		yield return new WaitForSeconds(secondsNormal);

		// emphasize distortion in background audio
		distorted.TransitionTo(0.1f);
		yield return new WaitForSeconds(2.9f);
		undistorted.TransitionTo(0.1f);
		pendingDistortion = false;
	}
}
