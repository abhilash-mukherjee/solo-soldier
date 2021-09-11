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
