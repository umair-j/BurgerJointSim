using System;
using UnityEngine;
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static event Action OnGameInitialize;
    public static event Action OnGameSave;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        InitializeGame();    
    }
    private void InitializeGame()
    {
        GameDataManager.Get();
        OnGameInitialize?.Invoke();
    }
    public void saveGame()
    {
        OnGameSave?.Invoke();
        GameDataManager.Set();
    }
}
