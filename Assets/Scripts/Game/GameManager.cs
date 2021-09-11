using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour, ISavable
{
    [SerializeField]
    private float enemyCheckTimeGap = 1f;
    public delegate void GameOverHandler();
    public static event GameOverHandler OnGameOver;
    public delegate void LevelFinishedHandler();
    public static event LevelFinishedHandler OnLevelFinished;
    public delegate void KeyCollectionHandler();
    public static event KeyCollectionHandler OnKeyCollected;
    public static GameManager Instance;
    public int highestLevelAchieved = 0;
    public bool isLastBattleWon;
    private int m_enemyCount = 0;
    public int currentGrenadeCout = 0;

    [System.Serializable]
    public struct Level
    {
        public string levelName;
        public int levelIndex;
    }

    [SerializeField]
    public List<Level> levelNames;
    [SerializeField]
    private string MAIN_MENU;
    [SerializeField]
    private int finalLevelIndex = 3;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadGameData();
        if(SceneManager.GetActiveScene().name != MAIN_MENU)
        {
            UpdateEnemyCount();
        }
    }


    private void OnEnable()
    {
        PlayerHealth.OnDied += LoadGameOver;
        EnemyAnimationManager.OnEnemyDied += CheckIfNoEnemyLeft;
        GunShipDeathManager.OnGunShipDied += CheckIfNoEnemyLeft;
        TankHealth.OnTankDied += CheckIfNoEnemyLeft;
    }


    private void OnDisable()
    {
        PlayerHealth.OnDied -= LoadGameOver;
        EnemyAnimationManager.OnEnemyDied -= CheckIfNoEnemyLeft;
        GunShipDeathManager.OnGunShipDied -= CheckIfNoEnemyLeft;
        TankHealth.OnTankDied -= CheckIfNoEnemyLeft;

    }

    private void OnApplicationQuit()
    {
        SaveGameData(); 
    }

    private void CheckIfNoEnemyLeft()
    {
        StartCoroutine(WaitAndCheckEnemyCount(enemyCheckTimeGap));
    }

    IEnumerator WaitAndCheckEnemyCount(float _time)
    {
        yield return new WaitForSeconds(_time);
        UpdateEnemyCount();
        Debug.Log("Enemy Count: " + m_enemyCount);
        if (m_enemyCount <= 0)
        {
            Debug.Log("Success");
            OnLevelFinished?.Invoke();
        }
    }
    private void GetGrenadeCount()
    {
        var _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            currentGrenadeCout = _player.GetComponent<PlayerGrenadeCounter>().GrenadeCount;
        }
    }
 

    private void UpdateEnemyCount()
    {
        GameObject[] _footEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] _gunShips = GameObject.FindGameObjectsWithTag("GunShip");
        GameObject[] _tanks = GameObject.FindGameObjectsWithTag("Tank");
        m_enemyCount = _footEnemies.Length + _gunShips.Length + _tanks.Length;
    }

    private void LoadGameOver(GameObject _gameObject)
    {
        isLastBattleWon = false;
        Debug.Log("GameOver");
        CacheLevelData();
        OnGameOver?.Invoke();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void UnlockNextLevel()
    {
        string _currentLevel = SceneManager.GetActiveScene().name;
        int _currentLevelIndex = GetLevelIndex(_currentLevel);
        if (_currentLevelIndex == highestLevelAchieved && _currentLevelIndex < finalLevelIndex)
        {
            highestLevelAchieved++;
            isLastBattleWon = true;
        }
        else
        {
            isLastBattleWon = false;
        }
        OnKeyCollected?.Invoke();
        CacheLevelData();
        LoadMainMenu();
    }
    private void CacheLevelData()
    {
        string _currentLevel = SceneManager.GetActiveScene().name;
        int _currentLevelIndex = GetLevelIndex(_currentLevel);
        if(_currentLevelIndex > highestLevelAchieved)
        {
            highestLevelAchieved = _currentLevelIndex;
        }
        GetGrenadeCount();
        
    }

    private int GetLevelIndex(string _levelName)
    {
        var _level = levelNames.Find(level => level.levelName == _levelName);
        return _level.levelIndex;

    }
    private void SaveGameData()
    {
        SaveData _saveData = new SaveData();
        PopulateSaveData(_saveData);
    }
    private void LoadGameData()
    {
        SaveData _saveData = new SaveData();
        LoadFromSaveData(_saveData);
        this.currentGrenadeCout = _saveData.m_grenadeCount;
    }

    public void PopulateSaveData(SaveData _saveData)
    {
        _saveData.m_grenadeCount = currentGrenadeCout;
        string _json = _saveData.ToJson();
        FileManager.WriteToFile("SaveData.dat", _json);

    }

    public void LoadFromSaveData(SaveData _saveData)
    {
        if(FileManager.LoadFromFile("SaveData.dat", out var _json))
        {
            _saveData.LoadFromJson(_json);
        }
    }
}
