using System;
using UnityEngine;

public class BullDozerTriggerController : MonoBehaviour
{
	#region Serialized Properties
	[SerializeField]
	private BullDozerAnimationController _bullDozerAnimationController;
	#endregion

	#region Private Members
	private const string TAG_NAME = "Truck";
	private DumpingTruckController _lastPawn;
	#endregion

	#region Private Methods
	private void Start()
	{
		if (_bullDozerAnimationController == null)
		{
			Debug.LogWarningFormat("[{0}][BullDozerAnimationController]_bullDozerAnimationController is NULL!", GetType().Name);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == TAG_NAME)
		{
			_lastPawn = other.gameObject.GetComponent<DumpingTruckController>();
			if (_lastPawn == null)
			{
				Debug.LogWarningFormat("[{0}][OnTriggerEnter]_lastPawn is NULL!", GetType().Name);
				return;
			}

			_lastPawn.IsLoadingCargo(true);
			Action callBack = ReleaseTruck;
			_bullDozerAnimationController.StartAnimation(callBack);
		}
	}

	private void ReleaseTruck()
	{
		_lastPawn.IsLoadingCargo(false);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == TAG_NAME)
		{
			if (_lastPawn == other.gameObject.GetComponent<DumpingTruckController>())
			{
				_lastPawn = null;
			}
		}
	}
	#endregion
}