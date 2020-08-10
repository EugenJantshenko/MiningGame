using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour, IInitializable
{
	#region Serialized Properties
	[SerializeField]
	private List<AudioClip> _audioClips;
	#endregion

	#region Private Members
	private AudioSource _audioSource;
	#endregion

	#region Public Methods
	public void Initialize()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlaySound(SoundEnum soundEnum)
	{
		_audioSource.PlayOneShot(_audioClips[(int)soundEnum]);
	}

	public void StopSound()
	{
		_audioSource.Stop();
	}
	#endregion
}