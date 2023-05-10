using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseCounter, IKitchenObjectParent
{
    private List<KitchenObject> kitchenObjects;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform objectSpawnPoint;

    //public Counter secondCounterForTesting;

    private void OnEnable()
    {
        GameController.OnGameInitialize += GameController_OnGameInitialize;
        GameController.OnGameSave += GameController_OnGameSave;
    }

    private void GameController_OnGameSave()
    {
        GameData.Instance.cookingCounterKitchenObjectsCount = kitchenObjects.Count;
    }

    private void GameController_OnGameInitialize()
    {
        KitchenCountersInit();
    }
    private void KitchenCountersInit()
    {
        if (kitchenObjects == null)
        {
            kitchenObjects = new List<KitchenObject>();
        }
        ClearKitchenObjects();

        PopulateCounter(GameData.Instance.cookingCounterKitchenObjectsCount);

    }
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
        foreach (KitchenObject kitchenObject in kitchenObjects)
        {
            Destroy(kitchenObject.gameObject);
        }
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
        if (Player.Instance.GetKitchenObjects().Count < MAX_STACKABLE_ITEMS)
        {
            if (kitchenObjects.Count < MAX_STACKABLE_ITEMS)
            {
                PopulateCounter();
            }
            else
            {
                Player.Instance.ClearKitchenObjects();
                for(int i = 0;i < kitchenObjects.Count; i++)
                {
                    kitchenObjects[i].SetKitchenObjectParent(Player.Instance);
                    Player.Instance.AddKitchenObjects(kitchenObjects[i]);
                    if (i == 0)
                    {
                        kitchenObjects[i].GetComponent<FollowObject>().UpdatePosition(Player.Instance.GetObjectSpawnPoint(), 20, true);
                    }
                    else
                    {
                        kitchenObjects[i].GetComponent<FollowObject>().UpdatePosition(kitchenObjects[i-1].transform, 20, true);
                    }
                }
                kitchenObjects.Clear();
                // max items stacked
            }
        }
    }
    public void PopulateCounter(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector3 placementOffset = new Vector3(0, i * kitchenObjectSO.objectHeight, 0);
            GameObject spawnedPizza = Instantiate(kitchenObjectSO.prefab, objectSpawnPoint);
            spawnedPizza.transform.localPosition = Vector3.zero;
            spawnedPizza.transform.position = spawnedPizza.transform.position + placementOffset;
            KitchenObject spawnedObj = spawnedPizza.GetComponent<KitchenObject>();
            kitchenObjects.Add(spawnedObj);
        }
        
    }
    public void PopulateCounter()
    {
            Vector3 placementOffset = new Vector3(0, kitchenObjects.Count * kitchenObjectSO.objectHeight, 0);
            GameObject spawnedPizza = Instantiate(kitchenObjectSO.prefab, objectSpawnPoint);
            spawnedPizza.transform.localPosition = Vector3.zero;
            spawnedPizza.transform.position = spawnedPizza.transform.position + placementOffset;
            KitchenObject spawnedObj = spawnedPizza.GetComponent<KitchenObject>();
            kitchenObjects.Add(spawnedObj);
    }


}
