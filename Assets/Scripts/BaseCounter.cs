using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour
{
    protected const int MAX_STACKABLE_ITEMS = 7;

    public abstract void Interact();
    public abstract List<KitchenObject> GetKitchenObjects();
}
