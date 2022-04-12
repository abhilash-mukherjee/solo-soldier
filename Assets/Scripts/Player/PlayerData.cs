using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData, ISavable
{
    private void OnEnable()
    {
        PlayerHealth.OnDied += AddToDeletedDataList;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDied -= AddToDeletedDataList;
    }
    public void LoadFromSaveData(SaveData sd)
    {
        if (sd.m_characterDataStructList != null)
        {
            foreach (SaveData.CharacterDataStruct _characterData in sd.m_characterDataStructList)
            {
                Debug.Log("List Length = " + sd.m_characterDataStructList.Count);
                Debug.Log("This id = " + m_uID);
                Debug.Log("Current list element id = " + _characterData.m_uID);
                if (_characterData.m_uID == this.m_uID)
                {
                    if (gameObject.GetComponent<PlayerHealth>() != null)
                    {
                        gameObject.GetComponent<PlayerHealth>().CurrentHealth = _characterData.m_health;
                        if (_characterData.m_health == 0)
                        {
                            Destroy(gameObject);
                            Debug.Log($"{this.m_uID } destroyed");
                        }
                    }
                    transform.SetPositionAndRotation(_characterData.m_position, _characterData.m_rotation);
                    return;
                }
                 
            }
        }
        
    }

    public void PopulateSaveData(SaveData sd)
    {
        if (sd.m_characterDataStructList != null)
        {
            var _character = sd.m_characterDataStructList.Find(_character => _character.m_uID == m_uID);
            if (_character.m_uID == "")
            {
                var _newCharacterData = GetCharacterDataStruct();
                sd.m_characterDataStructList.Add(_newCharacterData);
                Debug.Log($"Added {_newCharacterData.m_uID}");
            }
            else
            {
                Debug.Log($"Removed {_character.m_uID} and the current health of player is: {this.m_health}");
                sd.m_characterDataStructList.Remove(_character);
                var _characterData = GetCharacterDataStruct();
                sd.m_characterDataStructList.Add(_characterData);
                return;
            }
        }
        //UpdateDeletedDataStructList(sd);
    }
    public void LoadFromSaveDataClone(SaveData sd)
    {
        if (sd.m_characterDataStructDict != null)
        {
            if (sd.m_characterDataStructDict.ContainsKey(m_uID))
            {
                if (gameObject.GetComponent<PlayerHealth>() != null)
                {
                    gameObject.GetComponent<PlayerHealth>().CurrentHealth = sd.m_characterDataStructDict[m_uID].m_health;
                    if (sd.m_characterDataStructDict[m_uID].m_health == 0)
                    {
                        Destroy(gameObject);
                        Debug.Log($"{gameObject.name } destroyed");
                    }
                }
                transform.SetPositionAndRotation(sd.m_characterDataStructDict[m_uID].m_position, sd.m_characterDataStructDict[m_uID].m_rotation);
                return;
            }
            Debug.Log("LoadFromSaveDataCalled of " + gameObject.name);
        }
    }
    public void PopulateSaveDataClone(SaveData sd)
    {
        if (sd.m_characterDataStructDict.ContainsKey(m_uID))
        {
            sd.m_characterDataStructDict[m_uID] = GetCharacterDataStruct();
        }
        else
        {
            sd.m_characterDataStructDict.Add(m_uID, GetCharacterDataStruct());
        }
    }
    private SaveData.CharacterDataStruct GetCharacterDataStruct()
    {
        if (gameObject.GetComponent<PlayerHealth>() != null)
        {
            m_health = gameObject.GetComponent<PlayerHealth>().CurrentHealth;
        }
        m_position = transform.position;
        m_rotation = transform.rotation;
        SaveData.CharacterDataStruct _newCharacterData = new SaveData.CharacterDataStruct
        {
            m_health = this.m_health,
            m_position = this.m_position,
            m_rotation =this. m_rotation,
            m_uID = this.m_uID
        };
        return _newCharacterData;
    }
}