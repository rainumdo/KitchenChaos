using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
	[SerializeField] private KitchenObjectSO kitchenObjectSO;

	private IKitchenObjectParent kitchenObjectParent;

	public KitchenObjectSO GetKitchenObjectSO()
	{
		return kitchenObjectSO;
	}

	public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
	{
		//logic
		if (this.kitchenObjectParent != null)
		{
			this.kitchenObjectParent.ClearKitchenObject();
		}
		this.kitchenObjectParent = kitchenObjectParent;

		if (kitchenObjectParent.HasKitchenObject())
		{
			Debug.LogError("KitchenObjectParent already has a KitchenObject!");
		}
		kitchenObjectParent.SetKitchenObject(this);

		// visual
		transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
		transform.localPosition = Vector3.zero;
	}

	public IKitchenObjectParent GetKitchenObjectParent()
	{
		return kitchenObjectParent;
	}

	public void DestroySelf()
	{
		kitchenObjectParent.ClearKitchenObject();
		Destroy(gameObject);
	}

	public bool TryGetPlate(out PlateKitChenObject plateKitChenObject)
	{
		if (this is PlateKitChenObject)
		{
			plateKitChenObject = this as PlateKitChenObject;
			return true;
		}
		else
		{
			plateKitChenObject = null;
			return false;
		}
	}

	public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
	{
		Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
		KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
		kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
		return kitchenObject;
	}
}
