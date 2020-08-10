using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PathHolder : MonoBehaviour
{
	#region Serialized Properties
	[SerializeField]
	private GameObject _dumpingTruckPref;
	[SerializeField]
	[HideInInspector]
	private List<Transform> _points;
	#endregion

	#region Injected Fields
	[Inject]
	private PlayerProgress _playerProgress;
	#endregion

	#region Privae Members
	private DumpingTruckController _dumpingTruck;
	#endregion

	#region Constants
	private const string POINT_NAME = "PathPoint_";
	#endregion

	#region Private Methods
	private void Start()
	{
		AddDumpingTruck();
	}

	private void AddDumpingTruck()
	{
		_dumpingTruck = GameObject.Instantiate(_dumpingTruckPref).GetComponent<DumpingTruckController>();

		if (_dumpingTruck == null)
		{
			Debug.LogWarningFormat("[{0}][AddDumpingTruck]_dumpingTruck is NULL", GetType().Name);
			return;
		}

		if (_points != null && _points.Count > 0)
		{
			_dumpingTruck.transform.parent = this.transform;
			_dumpingTruck.SetParams(_playerProgress).Initialized(_points);
		}
		else
		{
			Debug.LogWarningFormat("[{0}][AddDumpingTruck]_points is NULL ore empty", GetType().Name);
		}
	}
	#endregion

	#region Public Methods
	public List<Transform> Points
	{
		get { return _points; }
	}

	public void InitializePoints()
	{
		if (_points == null)
		{
			_points = new List<Transform>();
		}
	}

	public void AddPoint()
	{
		Transform newPoint = new GameObject().transform;
		newPoint.transform.parent = this.gameObject.transform;
		InitializePoints();
		_points.Add(newPoint);
		newPoint.gameObject.name = string.Format("{0}{1}", POINT_NAME, _points.Count);
	}

	public void RemovePoint(int pointId)
	{
		if (_points != null && pointId >= 0 && pointId < _points.Count)
		{
			Transform point = _points[pointId];
			_points.RemoveAt(pointId);
			DestroyImmediate(point.gameObject);

			for (int i = 0; i < _points.Count; i++)
			{
				_points[i].name = string.Format("{0}{1}", POINT_NAME, i + 1);
			}
		}
		else
		{
			Debug.LogWarningFormat("[{0}][RemovePoint]_points is empty!", GetType().Name);
		}
	}
	#endregion
}