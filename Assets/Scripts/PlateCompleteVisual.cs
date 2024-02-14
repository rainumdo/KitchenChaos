using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
	[Serializable]
	public struct KitchenObjectSO_GameObject
	{
		public KitchenObjectSO kitchenObjectSO;
		public GameObject gameObject;
	}

	[SerializeField] private PlateKitChenObject plateKitChenObject;
	[SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;

	private void Start()
	{
		plateKitChenObject.OnIngredientAdded += PlateKitChenObject_OnIngredientAdded;
		foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList)
		{
			kitchenObjectSO_GameObject.gameObject.SetActive(false);
		}
	}

	private void PlateKitChenObject_OnIngredientAdded(object sender, PlateKitChenObject.OnIngredientAddedEventArgs e)
	{
		foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList)
		{
			if (kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO)
			{
				kitchenObjectSO_GameObject.gameObject.SetActive(true);
			}
		}
	}
}
