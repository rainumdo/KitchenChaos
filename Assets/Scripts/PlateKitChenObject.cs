using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitChenObject : KitchenObject
{
	public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
	public class OnIngredientAddedEventArgs : EventArgs
	{
		public KitchenObjectSO kitchenObjectSO;
	}

	[SerializeField] private List<KitchenObjectSO> validKitchenObjectList;

	private List<KitchenObjectSO> kitchenObjectList;

	private void Awake()
	{
		kitchenObjectList = new List<KitchenObjectSO>();
	}

	public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
	{
		if (!validKitchenObjectList.Contains(kitchenObjectSO))
		{
			// Not a valid ingredient
			return false;
		}

		if (kitchenObjectList.Contains(kitchenObjectSO))
		{
			return false;
		}
		else
		{
			kitchenObjectList.Add(kitchenObjectSO);

			OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
			{
				kitchenObjectSO = kitchenObjectSO
			});
			return true;
		}
	}

	public List<KitchenObjectSO> GetKitchenObjectSOList()
	{
		return kitchenObjectList;
	}
}
