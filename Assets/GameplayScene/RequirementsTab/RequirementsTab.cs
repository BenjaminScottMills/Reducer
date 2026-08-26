using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RequirementsTab : MonoBehaviour
{
    string levelPath;
    LevelData levelData;
    public LevelType Initialise(string levelPathArg)
    {
        levelPath = levelPathArg;
        string levelDataPath = Path.Combine(levelPath, "levelData.json");
        levelData = LevelData.MakeLevelData(File.ReadAllText(levelDataPath));

        return levelData.levelType;
    }
}
