using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int m_grenadeCount;
    public bool m_isLastBattleWon;
    public int m_highestLevelAchieved;
    public string m_levelPlayedWhileQuitting;
    public bool m_isKeySpawnned;
    public float m_gunShipTimer;
    [System.Serializable]
    public struct CharacterDataStruct
    {
        public string m_uID;
        public int m_health;
        public Vector3 m_position;
        public Quaternion m_rotation;
    }
    public struct DeletedDataStruct
    {
        public string m_uID;
    }
    public List<CharacterDataStruct> m_characterDataStructList = new List<CharacterDataStruct>();
    public Dictionary<string,CharacterDataStruct> m_characterDataStructDict = new Dictionary<string,CharacterDataStruct>();
    public List<string> m_deletedChatacters = new List<string>();
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public void LoadFromJson(string _json)
    {
        JsonUtility.FromJsonOverwrite(_json, this);
    }
}

public interface ISavable
{
    void PopulateSaveData(SaveData sd);
    void LoadFromSaveData(SaveData sd);
}
