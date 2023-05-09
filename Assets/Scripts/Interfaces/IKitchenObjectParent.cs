using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetObjectSpawnPoint();
    public List<KitchenObject> GetKitchenObjects();
    public void SetKitchenObjects(List<KitchenObject> kitchenObjList);
}
