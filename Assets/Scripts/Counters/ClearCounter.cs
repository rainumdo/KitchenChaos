using UnityEngine;

public class ClearCounter : BaseCounter
{
	[SerializeField] private KitchenObjectSO kitchenObjectSO;

	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			// There is no KitchenObject
			if (player.HasKitchenObject())
			{
				// player is carrying something
				player.GetKitchenObject().SetKitchenObjectParent(this);
			}
			else
			{
				// player is not carrying anything
			}
		}
		else
		{
			// There is a KitchenObject
			if (player.HasKitchenObject())
			{
				// player is carrying something
				if (player.GetKitchenObject().TryGetPlate(out PlateKitChenObject plateKitChenObject))
				{
					// Player is holding a Plate
					if (plateKitChenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
					{
						GetKitchenObject().DestroySelf();
					}
				}
				else
				{
					// Player is not carrying Plate but something else
					if (GetKitchenObject().TryGetPlate(out plateKitChenObject))
					{
						if (plateKitChenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
						{
							player.GetKitchenObject().DestroySelf();
							
						}
						
					}
				}
			}
			else
			{
				// player is not carrying anything
				this.GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}
}
