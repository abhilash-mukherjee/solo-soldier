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
    [SerializeField]
    private float playerCheckTimeGap = 1f;
    [SerializeField] private BoolVariable boolVariable;
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
    [HideInInspector]
    public int currentGrenadeCout = 0;
    [SerializeField]
    private int startingGrenadeount = 5;

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
        if (Instance != null)
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
        if (SceneManager.GetActiveScene().name != MAIN_MENU)
        {
            UpdateEnemyCount();
        }
    }


    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += LoadGameOver;
        EnemyAnimationManager.OnEnemyDied += CheckIfNoEnemyLeft;
        GunShipDeathManager.OnGunShipDied += CheckIfNoEnemyLeft;
        TankHealth.OnTankDied += CheckIfNoEnemyLeft;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoaded;
    }


    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= LoadGameOver;
        EnemyAnimationManager.OnEnemyDied -= CheckIfNoEnemyLeft;
        GunShipDeathManager.OnGunShipDied -= CheckIfNoEnemyLeft;
        TankHealth.OnTankDied -= CheckIfNoEnemyLeft;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLoaded;
    }

    private void OnLoaded(Scene scene, LoadSceneMode arg1)
    {
        boolVariable.Value = false;
        Time.timeScale = 1f;
        Debug.Log(scene.name);
        Debug.Log($"Deleted charaacter list count: {CharacterData.m_deletedCharacters.Count()}");
        
    }

    private void OnApplicationQuit()
    {

        SaveGameData();
    }
  
    private void CheckIfNoEnemyLeft()
    {
        if (SceneManager.GetActiveScene().name != MAIN_MENU)
        {
            StartCoroutine(WaitAndCheckEnemyCount(enemyCheckTimeGap));
        }
    }
    IEnumerator CheckIfPlayerDied( float _time)
    {
        yield return new WaitForSeconds(_time);
        Debug.Log("Checking Player Count");
        if (SceneManager.GetActiveScene().name != MAIN_MENU)
        {
            var _player = GameObject.FindGameObjectWithTag("Player");
            if(_player == null)
            {
                Debug.Log("Player Died");
                UnlockCursor();
                LoadMainMenu();
            }
            else if(_player.GetComponent<PlayerHealth>() == null)
            {
                Debug.Log("Player Died");
                UnlockCursor();
                LoadMainMenu();
            }
            else
            {
                Debug.Log(_player);
            }
        }
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
    private void LoadGameOver()
    {
        isLastBattleWon = false;
        Debug.Log("GameOver");
        CacheLevelData();
        OnGameOver?.Invoke();
    }
    public void LoadMainMenu()
    {
        
        EmptyDeletedCharacterList();
        DeleteUnnecesseryCharacterDataFromCurentScene();
        SceneManager.LoadScene(MAIN_MENU,LoadSceneMode.Single);
        
    }
    private void EmptyDeletedCharacterList()
    {
        CharacterData.m_deletedCharacters.Clear();
        Debug.Log($"DeleedCharacter List empty function: Expected List size = 0, actual list size in runtime = {CharacterData.m_deletedCharacters.Count()}");
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
        if (_currentLevelIndex > highestLevelAchieved)
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
        GetGrenadeCount();
        SaveData _saveData = new SaveData();
        if (FileManager.LoadFromFile("SaveData.dat", out var _json))
        {
            _saveData.LoadFromJson(_json);
        }
        PopulateSaveData(_saveData);
    }
    private void LoadGameData()
    {
        SaveData _saveData = new SaveData();
        LoadFromSaveData(_saveData);
        if (_saveData.m_levelPlayedWhileQuitting == null)
        {
            Debug.Log("This is first gamePlay");
            SetDefaultValues();
            return;
        }
        this.currentGrenadeCout = _saveData.m_grenadeCount;
        this.isLastBattleWon = _saveData.m_isLastBattleWon;
        this.highestLevelAchieved = _saveData.m_highestLevelAchieved;
        SceneManager.LoadScene(_saveData.m_levelPlayedWhileQuitting);
        StartCoroutine(LoadDataAfterSceneLoad(_saveData));
        

    }
    private void SetDefaultValues()
    {
        currentGrenadeCout = startingGrenadeount;
        var _playerObj = GameObject.FindGameObjectWithTag("Player");
        if (_playerObj != null)
        {
            _playerObj.GetComponent<PlayerGrenadeCounter>().SetGrenadeCount(currentGrenadeCout);
        }
    }
    IEnumerator LoadDataAfterSceneLoad(SaveData _saveData)
    {
        while (SceneManager.GetActiveScene().name != _saveData.m_levelPlayedWhileQuitting)
        {
            yield return null;
        }

        // Do anything after proper scene has been loaded
        if (SceneManager.GetActiveScene().name == _saveData.m_levelPlayedWhileQuitting)
        {
            LoadCharacterData("Player", _saveData);
            LoadCharacterData("Enemy", _saveData);
            LoadCharacterData("Tank", _saveData);
            LoadCharacterData("GunShip", _saveData);
            if(SceneManager.GetActiveScene().name == "Level 2" || SceneManager.GetActiveScene().name == "Level 3")
            {
                GunShipTimer.gunShipTimers[0].LoadFromSaveData(_saveData);
                Debug.Log("Loas after scene load, gunshiptimer");
            }
            StartCoroutine(CheckIfPlayerDied(playerCheckTimeGap));
            CheckIfNoEnemyLeft();
        }
    }
    private void LoadCharacterData(string v, SaveData _saveData)
    {
        var _characters = GameObject.FindGameObjectsWithTag(v);
        if (_characters != null)
        {
            foreach (GameObject _character in _characters)
            {
                if (v == "Enemy")
                {

                    if (_character.GetComponent<EnemyData>() != null)
                    {
                        _character.GetComponent<EnemyData>().LoadFromSaveData(_saveData);
                    }
                }

                else if (v == "Player")
                {

                    if (_character.GetComponent<PlayerData>() != null)
                    {
                        _character.GetComponent<PlayerData>().LoadFromSaveData(_saveData);
                    }
                }
                else if (v == "GunShip")
                {

                    if (_character.GetComponent<GunShipData>() != null)
                    {
                        _character.GetComponent<GunShipData>().LoadFromSaveData(_saveData);
                    }
                }
                else if (v == "Tank")
                {

                    if (_character.GetComponent<TankData>() != null)
                    {
                        _character.GetComponent<TankData>().LoadFromSaveData(_saveData);
                    }
                }
            }
        }
    }
    public void PopulateSaveData(SaveData _saveData)
    {
        _saveData.m_grenadeCount = currentGrenadeCout;
        _saveData.m_isLastBattleWon = isLastBattleWon;
        _saveData.m_highestLevelAchieved = highestLevelAchieved;
        _saveData.m_levelPlayedWhileQuitting = SceneManager.GetActiveScene().name;
        PopulateCharacterData("Player", _saveData);
        PopulateCharacterData("Tank", _saveData); 
        PopulateCharacterData("Enemy", _saveData);
        PopulateCharacterData("GunShip", _saveData);
        PopulateDeletedCharacterData(_saveData);
        DeleteUnnecesseryCharacterData(_saveData);
        if (GunShipTimer.gunShipTimers.Count() != 0)
        {
            GunShipTimer.gunShipTimers[0].PopulateSaveData(_saveData);
        }
        string _json = _saveData.ToJson();
        FileManager.WriteToFile("SaveData.dat", _json);

    }
    // 5:43-6:43    : Game Dev
    private void PopulateCharacterData(string v, SaveData _saveData)
    {
        var _characters = GameObject.FindGameObjectsWithTag(v);
        if (_characters != null)
        {
            foreach (GameObject _character in _characters)
            {
                if (v == "Enemy")
                {

                    if (_character.GetComponent<EnemyData>() != null)
                    {
                        _character.GetComponent<EnemyData>().PopulateSaveData(_saveData);
                    }
                }

                else if (v == "Player")
                {

                    if (_character.GetComponent<PlayerData>() != null)
                    {
                        _character.GetComponent<PlayerData>().PopulateSaveData(_saveData);
                    }
                }
                else if (v == "GunShip")
                {

                    if (_character.GetComponent<GunShipData>() != null)
                    {
                        _character.GetComponent<GunShipData>().PopulateSaveData(_saveData);
                    }
                }
                else if (v == "Tank")
                {

                    if (_character.GetComponent<TankData>() != null)
                    {
                        _character.GetComponent<TankData>().PopulateSaveData(_saveData);
                    }
                }
            }
        }
    }
    private void PopulateDeletedCharacterDataClone(SaveData _saveData)
    {
        GetRecentlyDeletedCharacters(_saveData);
        foreach (var _characteID in _saveData.m_deletedChatacters)
        {
            SaveData.CharacterDataStruct _characterData = new SaveData.CharacterDataStruct
            {
                m_health = 0,
                m_uID = _characteID,
                m_position = Vector3.zero,
                m_rotation = Quaternion.identity
            };
            if (_saveData.m_characterDataStructDict.ContainsKey(_characteID) == false)
            {

                _saveData.m_characterDataStructDict.Add(_characteID, _characterData);
            }
        }
    }
    private void PopulateDeletedCharacterData(SaveData _saveData)
    {
        _saveData.m_deletedChatacters.Clear();
        GetRecentlyDeletedCharacters(_saveData);
        foreach (var _characterID in _saveData.m_deletedChatacters)
        {
            if(_saveData.m_characterDataStructList.Any(character => character.m_uID == _characterID))
            {
                var _characterData = _saveData.m_characterDataStructList.Find(character => character.m_uID == _characterID);
                _saveData.m_characterDataStructList.Remove(_characterData);
                SaveData.CharacterDataStruct _newCharacterData = new SaveData.CharacterDataStruct
                {
                    m_health = 0,
                    m_uID = _characterID,
                    m_position = Vector3.zero,
                    m_rotation = Quaternion.identity 
                };
                _saveData.m_characterDataStructList.Add(_newCharacterData);
                Debug.Log($"Updated {_newCharacterData.m_uID} healh in Character Data list");
            }
            else
            {
                SaveData.CharacterDataStruct _characterData = new SaveData.CharacterDataStruct
                {
                    m_health = 0,
                    m_uID = _characterID,
                    m_position = Vector3.zero, 
                    m_rotation = Quaternion.identity
                };
                _saveData.m_characterDataStructList.Add(_characterData);
                Debug.Log ($"Added {_characterData.m_uID} to Character Data list");

            }
        }
    }
    private void GetRecentlyDeletedCharacters(SaveData _saveData)
    {
        foreach (var id in CharacterData.m_deletedCharacters)
        {
            if (_saveData.m_deletedChatacters.Contains(id) == false)
                _saveData.m_deletedChatacters.Add(id);
        }
        var _dataToRemove = new List<string>();
        foreach (var id in _saveData.m_deletedChatacters)
        {
            if (id.Contains(SceneManager.GetActiveScene().name) == false)
            {
                _dataToRemove.Add(id);
            }
        }
        foreach(var id in _dataToRemove)
        {
            _saveData.m_deletedChatacters.Remove(id);
            Debug.Log($"Removed {id} from deleted Character list");
        }
    }
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadFromSaveData(SaveData _saveData)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var _json))
        {
            _saveData.LoadFromJson(_json);
        }
    }
    private void DeleteUnnecesseryCharacterData(SaveData _saveData)
    {
        Debug.Log("Inside DeleteUnnecessaryData()");
        var _dataToRemove = new List<SaveData.CharacterDataStruct>();
        foreach(var _character in _saveData.m_characterDataStructList)
        {
            if(_character.m_uID.Contains(SceneManager.GetActiveScene().name) == false)
            {
                _dataToRemove.Add(_character);
            }
        }

        foreach (var _character in _dataToRemove)
        {
            var _characterToRemove = _saveData.m_characterDataStructList.Find(p => p.m_uID == _character.m_uID);
            int initialLength = _saveData.m_characterDataStructList.Count();
            _saveData.m_characterDataStructList.Remove(_characterToRemove);
            int finalalLength = _saveData.m_characterDataStructList.Count();
            Debug.Log($"Removed {_characterToRemove.m_uID} : {_characterToRemove.m_health} from m_characterDataStructList. " +
                $"Initial length = {initialLength} final length = {finalalLength}");
        }
    }
    private void DeleteUnnecesseryCharacterDataFromCurentScene()
    {
        SaveData _saveData = new SaveData();
        if (FileManager.LoadFromFile("SaveData.dat", out var _json))
        {
            _saveData.LoadFromJson(_json);
        }
        Debug.Log("Inside DeleteUnnecessaryData()");
        var _dataToRemove = new List<SaveData.CharacterDataStruct>();
        foreach(var _character in _saveData.m_characterDataStructList)
        {
            if(_character.m_uID.Contains(SceneManager.GetActiveScene().name) == true)
            {
                _dataToRemove.Add(_character);
            }
        }

        foreach (var _character in _dataToRemove)
        {
            var _characterToRemove = _saveData.m_characterDataStructList.Find(p => p.m_uID == _character.m_uID);
            int initialLength = _saveData.m_characterDataStructList.Count();
            _saveData.m_characterDataStructList.Remove(_characterToRemove);
            int finalalLength = _saveData.m_characterDataStructList.Count();
            Debug.Log($"Removed {_characterToRemove.m_uID} : {_characterToRemove.m_health} from m_characterDataStructList. " +
                $"Initial length = {initialLength} final length = {finalalLength}");
        }

        _json = _saveData.ToJson();
        FileManager.WriteToFile("SaveData.dat", _json);

    } 
}
