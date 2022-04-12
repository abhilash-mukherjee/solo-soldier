using UnityEngine;

[CreateAssetMenu(fileName ="New Bool Variable", menuName ="Variables/ Bool Variable")]
public class BoolVariable : ScriptableObject
{
    [SerializeField]
    private bool value;
    public bool Value { get => value; set => this.value = value; }
}
