using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyData : CharacterData, ISavable
{
    private void OnEnable()
    {
        EnemyHealth.OnDied += AddToDeletedDataList;
    }
    private void OnDisable()
    {
        EnemyHealth.OnDied -= AddToDeletedDataList;
    }

    public void LoadFromSaveData(SaveData sd)
    {
        if (sd.m_characterDataStructList != null)
        {
            foreach (SaveData.CharacterDataStruct _characterData in sd.m_characterDataStructList)
            {
                if (_characterData.m_uID == this.m_uID)
                {
                    if (gameObject.GetComponent<EnemyHealth>() != null)
                    {
                        gameObject.GetComponent<EnemyHealth>().CurrentHealth = _characterData.m_health;
                        if (_characterData.m_health == 0)
                        {
                            Destroy(gameObject);
                            Debug.Log($"{gameObject.name } destroyed");
                        }
                    }
                    transform.SetPositionAndRotation(_characterData.m_position, _characterData.m_rotation);
                    return;
                }
            }
            Debug.Log("LoadFromSaveDataCalled of " + gameObject.name);
        }
    }
    public void PopulateSaveData(SaveData sd)
    {
        if (sd.m_characterDataStructList != null)
        {
            var _character = sd.m_characterDataStructList.Find(_character => _character.m_uID == m_uID);
            if(_character.m_uID == "")
            {
                var _newCharacterData = GetCharacterDataStruct();
                sd.m_characterDataStructList.Add(_newCharacterData);
            }
            else
            {
                sd.m_characterDataStructList.Remove(_character);
                var _characterData = GetCharacterDataStruct();
                sd.m_characterDataStructList.Add(_characterData);
                return;
            }
        }   
    }
    public void LoadFromSaveDataClone(SaveData sd)
    {
        if (sd.m_characterDataStructDict != null)
        {
            if(sd.m_characterDataStructDict.ContainsKey(m_uID))
            {
                if (gameObject.GetComponent<EnemyHealth>() != null)
                {
                    gameObject.GetComponent<EnemyHealth>().CurrentHealth = sd.m_characterDataStructDict[m_uID].m_health;
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
            sd.m_characterDataStructDict.Add(m_uID,GetCharacterDataStruct());
        }
    }

    private SaveData.CharacterDataStruct GetCharacterDataStruct()
    {
        if (gameObject.GetComponent<EnemyHealth>() != null)
        {
            m_health = gameObject.GetComponent<EnemyHealth>().CurrentHealth;
        }
        m_position = transform.position;
        m_rotation = transform.rotation;
        SaveData.CharacterDataStruct _newCharacterData = new SaveData.CharacterDataStruct
        {
            m_health = m_health,
            m_position = m_position,
            m_rotation = m_rotation,
            m_uID = m_uID
        };
        return _newCharacterData;
    }
}
