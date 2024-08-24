using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private Level level;

    private void Start()
    {
        level.OnLevelGenerationCompleted += HandleLevelGenerationCompleted;
    }

    private void HandleLevelGenerationCompleted()
    {
        float levelHeight = level.TotalLevelHeight;
        Debug.Log("Total Level Height after generation: " + levelHeight);
    }

    private void OnDestroy()
    {
        level.OnLevelGenerationCompleted -= HandleLevelGenerationCompleted;
    }
}
