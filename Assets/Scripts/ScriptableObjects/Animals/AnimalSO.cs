using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AnimalSO", fileName = "Nature/Animal")]
public class AnimalSO : ScriptableObject
{
	public float GrowTime;
	public int GrowthRatio;

	public float IdleMinTime;
	public float IdleMaxTime;

	public float WalkMinTime;
	public float WalkMaxTime;

	public float EatMinTime;
	public float EatMaxTime;
}
