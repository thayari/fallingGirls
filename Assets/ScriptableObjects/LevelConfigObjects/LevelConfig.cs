using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SegmentInfo
{
    public GameObject prefab;
    public int count;
}

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private List<SegmentInfo> sequence;

    public List<SegmentInfo> GetSequence()
    {
        return sequence;
    }
}
