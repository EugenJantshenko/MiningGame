
using UnityEngine;

[CreateAssetMenu(menuName ="Configs/ItemConfig",fileName = "New Item")]
public class ItemConfig : ScriptableObject
{
	#region Properties
	[Tooltip("Уровень Бульдозера")]
	[SerializeField] private int _level;
	public int Level { get => _level; set => _level = value; }

	[Tooltip("Начальная цена")]
	[SerializeField] private int _startUpdatePrice;
	public int StartUpdatePrice { get => _startUpdatePrice; set => _startUpdatePrice = value; }

	[Tooltip("Количество денег")]
	[SerializeField] private int _currentCoin;
	public int СurrentCoin { get => _currentCoin; set => _currentCoin = value; }
	#endregion
}
