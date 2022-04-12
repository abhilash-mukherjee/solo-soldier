using UnityEngine;
[CreateAssetMenu(fileName ="New Float Variable", menuName ="Variables/ Float Variable")]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float value;
    public float Value { get => value; set => this.value = value; }
}
