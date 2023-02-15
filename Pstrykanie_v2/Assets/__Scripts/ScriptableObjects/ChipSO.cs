using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chip_", menuName = "Chip/ChipSO")]
public class ChipSO : ScriptableObject
{
    public Sprite Image;
    public bool isMetal = false;
    public Vector3 size = new Vector3(3f, 1f, 3f);
    public float speed = 3000f;
    public float maxStretch = 10f;
    public float pushPower = 0.2f;
    public float mass = 10f;
    public PhysicMaterial physicMaterial;
}
