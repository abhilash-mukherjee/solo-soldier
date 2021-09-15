using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CharacterData : MonoBehaviour
{
    [HideInInspector]
    public string m_uID;
    [HideInInspector]
    public int m_health;
    [HideInInspector]
    public Vector3 m_position;
    [HideInInspector]
    public Quaternion m_rotation;

    public static List<string> m_deletedCharacters;
    private void Awake()
    {
        if (m_deletedCharacters == null)
        {
            m_deletedCharacters = new List<string>();
        }
        m_uID = gameObject.name + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
    protected void AddToDeletedDataList(GameObject _gameObject)
    {
        if (_gameObject != this.gameObject)
            return;
        Debug.Log($"added {m_uID} to deleted character list");
        m_deletedCharacters.Add(m_uID);
    }
    //protected bool CheckIfDataIsDeleted(SaveData _saveData)
    //{
    //    foreach (SaveData.DeletedDataStruct _deletedData in _saveData.m_characterDataStructListDeleted)
    //    {
    //        if (this.m_uID == _deletedData.m_uID)
    //        {
    //            Debug.Log($"{gameObject.name} was deleted");
    //            Destroy(gameObject);
    //            return true;
    //        }
    //    }
    //    return false;
    //}

}
