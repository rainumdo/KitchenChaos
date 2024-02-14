using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRicipeSO : ScriptableObject
{
	public KitchenObjectSO input;
	public KitchenObjectSO output;
	public float burningTimerMax;
}
