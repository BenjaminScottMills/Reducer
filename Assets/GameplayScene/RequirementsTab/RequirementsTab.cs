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
    List<TestCase> customTestCases;
    public LevelType Initialise(string levelPathArg)
    {
        levelPath = levelPathArg;
        string levelDataPath = Path.Combine(levelPath, "levelData.json");
        levelData = LevelData.MakeLevelData(File.ReadAllText(levelDataPath));
        string customTestCasesPath = Path.Combine(levelPath, "customTests.json");
        if (Directory.Exists(customTestCasesPath))
        {
            customTestCases = JsonUtility.FromJson<CustomTestCasesSerialise>(File.ReadAllText(levelDataPath)).GetTestCases(levelData.levelType);
        }
        else
        {
            customTestCases = new();
        }

        return levelData.levelType;
    }
}
