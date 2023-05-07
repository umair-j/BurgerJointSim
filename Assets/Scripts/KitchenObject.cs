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
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        this.kitchenObjectParent = kitchenObjectParent;
        Vector3 kitchenObjectNewPosition = kitchenObjectParent.GetObjectSpawnPoint().position;
        transform.position = new Vector3(kitchenObjectNewPosition.x, transform.position.y, kitchenObjectNewPosition.z);
        transform.SetParent(kitchenObjectParent.GetObjectSpawnPoint());
    }
}
