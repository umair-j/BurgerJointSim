using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseCounter, IKitchenObjectParent
{
    private List<KitchenObject> kitchenObjects;
    [SerializeField] private KitchenObjectSO pizza;
    [SerializeField] private Transform objectSpawnPoint;

    //public Counter secondCounterForTesting;
    private void Start()
    {
        kitchenObjects = new List<KitchenObject>();
    }
//#if UNITY_EDITOR
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            if (secondCounterForTesting != null)
//            {
//                secondCounterForTesting.SetKitchenObjects(kitchenObjects);
//                kitchenObjects.Clear();
//            }
//        }
//    }
//#endif
    public Transform GetObjectSpawnPoint()
    {
        return objectSpawnPoint;
    }
    
    public override List<KitchenObject> GetKitchenObjects()
    {
        return kitchenObjects;
    }
    public void ClearKitchenObjects()
    {
        kitchenObjects.Clear();

    }
    public void SetKitchenObjects(List<KitchenObject> kitchenObjList)
    {
        kitchenObjects.Clear();
        kitchenObjects = new List<KitchenObject>(kitchenObjList);
        foreach(KitchenObject kitchenObj in kitchenObjects)
        {
            kitchenObj.SetKitchenObjectParent(this);
        }
    }
    public override void Interact()
    {
        int kitchenObjectsCount = kitchenObjects.Count;
        if (kitchenObjectsCount < MAX_STACKABLE_ITEMS)
        {
            Vector3 placementOffset = new Vector3(0, kitchenObjectsCount * pizza.objectHeight, 0);
            GameObject spawnedPizza = Instantiate(pizza.prefab, objectSpawnPoint);
            spawnedPizza.transform.localPosition = Vector3.zero;
            spawnedPizza.transform.position = spawnedPizza.transform.position + placementOffset;
            KitchenObject spawnedObj = spawnedPizza.GetComponent<KitchenObject>();
            kitchenObjects.Add(spawnedObj);
        }
        else
        {
            Player.Instance.ClearKitchenObjects();
            foreach (KitchenObject kitchenObj in kitchenObjects)
            {
                kitchenObj.SetKitchenObjectParent(Player.Instance);
                Player.Instance.AddKitchenObjects(kitchenObj);
            }
            kitchenObjects.Clear();
            // max items stacked
        }

    }

    
}
