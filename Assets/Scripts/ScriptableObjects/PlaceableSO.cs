using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSO : ScriptableObject
{
	public string Name;
	[TextArea(2, 4)]
	public string Description;

	public float DelayBetweenSpawnTime;
	public int MoneyToSpawn;
	public int MoneyReward;

	public Sprite Sprite;

	public GameObject TransparentObjectIndicator;

	public GameObject PlacedPrefab;
}
