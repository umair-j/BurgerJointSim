using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact()
    {
        int kitchenObjectsCount = Player.Instance.GetKitchenObjects().Count;
        if (kitchenObjectsCount == MAX_STACKABLE_ITEMS)
        {
            Player.Instance.ClearKitchenObjects();
        }
        else
        {
            // not enough pizzas for delivery
        }
    }
}
