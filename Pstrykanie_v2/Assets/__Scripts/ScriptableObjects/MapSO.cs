using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map_", menuName = "Map/MapSO")]
public class MapSO : ScriptableObject
{
    public string mapSceneName;
    public string mapName;
    public Sprite mapIcon;
}
