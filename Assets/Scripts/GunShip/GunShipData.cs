using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipData : CharacterData, ISavable
{
    private void OnEnable()
    {
        GunShipHealth.OnDied += AddToDeletedDataList;
    }
    private void OnDisable()
    {
        GunShipHealth.OnDied -= AddToDeletedDataList;
    }
    public void LoadFromSaveData(SaveData sd)
    {
        if (sd.m_characterDataStructList != null)
        {
            foreach (SaveData.CharacterDataStruct _characterData in sd.m_characterDataStructList)
            {
                if (_characterData.m_uID == this.m_uID)
                {
                    if (gameObject.GetComponent<GunShipHealth>() != null)
                    {
                        gameObject.GetComponent<GunShipHealth>().CurrentHealth = _characterData.m_health;
                        if (_characterData.m_health == 0)
                        {
                            gameObject.SetActive(false);
                            Destroy(gameObject);
                            Debug.Log($"{gameObject.name } destroyed");
                        }
                    }
                    transform.SetPositionAndRotation(_characterData.m_position, _characterData.m_rotation);
                }
            }
        }
    }

    public void PopulateSaveData(SaveData sd)
    {
        if (sd.m_characterDataStructList != null)
        {
            foreach (SaveData.CharacterDataStruct _data in sd.m_characterDataStructList)
            {
                if (_data.m_uID == this.m_uID)
                {
                    sd.m_characterDataStructList.Remove(_data);
                    var _characterData = GetCharacterDataStruct();
                    sd.m_characterDataStructList.Add(_characterData);
                    return;
                }
            }
        }
        var _newCharacterData = GetCharacterDataStruct();
        sd.m_characterDataStructList.Add(_newCharacterData);
       // UpdateDeletedDataStructList(sd);
    }
    public void LoadFromSaveDataClone(SaveData sd)
    {
        if (sd.m_characterDataStructDict != null)
        {
            if (sd.m_characterDataStructDict.ContainsKey(m_uID))
            {
                if (gameObject.GetComponent<GunShipHealth>() != null)
                {
                    gameObject.GetComponent<GunShipHealth>().CurrentHealth = sd.m_characterDataStructDict[m_uID].m_health;
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
        if (gameObject.GetComponent<GunShipHealth>() != null)
        {
            m_health = gameObject.GetComponent<GunShipHealth>().CurrentHealth;
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