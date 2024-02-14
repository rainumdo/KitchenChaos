using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
	public static DeliveryCounter Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public override void Interact(Player player)
	{
		if (player.HasKitchenObject())
		{
			if (player.GetKitchenObject().TryGetPlate(out PlateKitChenObject plateKitChenObject))
			{
				// Only accepts Plates
				DeliveryManager.Instance.DeliverRecipe(plateKitChenObject);

				player.GetKitchenObject().DestroySelf();
			}
		}
	}

}
