using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    public GameInput gameInput;
    private bool isWalking;
    [SerializeField]private float speed;
    [SerializeField] private float rotationSpeed;
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;
    private float playerRaycastDistance = 0.4f;
    private float interactionDistance = 2f;
    private Vector3 lastInteractDir;
    BaseCounter selectedCounter;
    [SerializeField]private Transform objectHoldPoint;
    private List<KitchenObject> kitchenObjects;
    private const int MAX_STACKABLE_ITEMS = 7;

    [SerializeField]private KitchenObjectSO kitchenObjectSO;
    public static Player Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    private void OnEnable()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        GameController.OnGameInitialize += GameController_OnGameInitialize;
        GameController.OnGameSave += GameController_OnGameSave;
        kitchenObjects = new List<KitchenObject>();
    }
    private void OnDisable()
    {
        gameInput.OnInteractAction -= GameInput_OnInteractAction;
        GameController.OnGameInitialize -= GameController_OnGameInitialize;
        GameController.OnGameSave -= GameController_OnGameSave;
    }
    private void KitchenCountersInit()
    {
        if (kitchenObjects == null)
        {
            kitchenObjects = new List<KitchenObject>();
        }
        ClearKitchenObjects();

        for (int i = 0; i < GameData.Instance.playerKitchenObjectsCount;i++)
        {
            Vector3 placementOffset = new Vector3(0, i * kitchenObjectSO.objectHeight, 0);
            GameObject spawnedPizza = Instantiate(kitchenObjectSO.prefab, objectHoldPoint);
            spawnedPizza.transform.localPosition = Vector3.zero;
            spawnedPizza.transform.position = spawnedPizza.transform.position + placementOffset;
            KitchenObject spawnedObj = spawnedPizza.GetComponent<KitchenObject>();
            kitchenObjects.Add(spawnedObj);
        }
        
    }
    private void GameController_OnGameSave()
    {
        GameData.Instance.playerPosition = transform.position;
        GameData.Instance.playerRotation = transform.rotation.eulerAngles;
        GameData.Instance.playerKitchenObjectsCount = kitchenObjects.Count;
    }
    IEnumerator SetPlayerRotationCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        SetPlayerRotation(GameData.Instance.playerRotation);
    }
    private void GameController_OnGameInitialize()
    {
        SetPosition(GameData.Instance.playerPosition);
        KitchenCountersInit();
        StartCoroutine(SetPlayerRotationCoroutine());
    }

    private void SetPlayerRotation(Vector3 playerRotation)
    {
        transform.eulerAngles = GameData.Instance.playerRotation; 
    }

    private void SetPosition(Vector3 playerPosition)
    {
        transform.position = playerPosition;
    }

    private void GameInput_OnInteractAction()
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }
    void HandleInteraction()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(InputVector.x, 0, InputVector.y);
        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        bool isInteracting = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hitinfo, interactionDistance);
        if (isInteracting)
        {
            if (hitinfo.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                if(selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }

            }
            else
            {
                selectedCounter = null;
                SetSelectedCounter(null);
            }
        }
        else
        {
            selectedCounter = null;
            SetSelectedCounter(null);
        }
    }
    void HandleMovement()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(InputVector.x, 0, InputVector.y);
        if (moveDir == Vector3.zero)
        {
            StopSwayKitchenItemVisual();
        }
        else if (kitchenObjects.Count != 0)
        {
            SwayKitchenItemVisual();
        }

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, playerRaycastDistance);
        if (canMove)
        {
            transform.position += moveDir * speed * Time.deltaTime;
        }
        else
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, playerRaycastDistance);
            if(canMove)
            {
                transform.position += moveDirX * speed * Time.deltaTime;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, playerRaycastDistance);
                if (canMove)
                {
                    transform.position += moveDirZ * speed * Time.deltaTime;
                }
                else
                {
                    // Player is stuck    
                }

            }
            
        }
        
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        isWalking = moveDir != Vector3.zero;
    }
    private void SwayKitchenItemVisual()
    {
        float swayMultiplier = 0.2f;
        int count = 0;
        foreach (KitchenObject kitchenObject in kitchenObjects)
        {
            Vector3 swayVector = new Vector3(kitchenObject.transform.localPosition.x, kitchenObject.transform.localPosition.y, GetObjectSpawnPoint().localPosition.z + count * swayMultiplier * -1);
            kitchenObject.transform.localPosition = Vector3.Lerp(kitchenObject.transform.localPosition, swayVector, 0.1f);
            count++;
        }

    }
    private void StopSwayKitchenItemVisual()
    {
        foreach (KitchenObject kitchenObject in kitchenObjects)
        {
            Vector3 restPositionVector = new Vector3(kitchenObject.transform.localPosition.x, kitchenObject.transform.localPosition.y, GetObjectSpawnPoint().localPosition.z);
            kitchenObject.transform.localPosition = Vector3.Lerp(kitchenObject.transform.localPosition, restPositionVector, 0.1f);
        }
    }
    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = counter
        }
        );
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    public Transform GetObjectSpawnPoint()
    {
        return objectHoldPoint;
    }
    public List<KitchenObject> GetKitchenObjects()
    {
        return kitchenObjects;
    }
    public void SetKitchenObjects(List<KitchenObject> kitchenObjList)
    {
        kitchenObjects.Clear();
        kitchenObjects = new List<KitchenObject>(kitchenObjList);
        foreach (KitchenObject kitchenObj in kitchenObjects)
        {
            kitchenObj.SetKitchenObjectParent(this);
        }
    }
    
    public void ClearKitchenObjects()
    {
        foreach(KitchenObject kitchenObject in kitchenObjects)
        {
            Destroy(kitchenObject.gameObject);
        }
        kitchenObjects.Clear();
    }
    public void AddKitchenObjects(KitchenObject kitchenObject)
    {
        kitchenObjects.Add(kitchenObject);
    }
    
}
