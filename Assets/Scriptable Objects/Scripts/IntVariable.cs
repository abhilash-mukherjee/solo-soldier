using UnityEngine;

[CreateAssetMenu(fileName ="New Float Variable", menuName ="Variables/ Int Variable")]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int value;
    public int Value { get => value; set => this.value = value; }
}
