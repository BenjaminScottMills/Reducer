using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        if (File.Exists(customTestCasesPath))
        {
            customTestCases = JsonUtility.FromJson<CustomTestCasesSerialise>(File.ReadAllText(levelDataPath)).GetTestCases(levelData.levelType);
        }
        else
        {
            customTestCases = new();
        }

        return levelData.levelType;
    }

    public void WriteCustomTestCases()
    {
        string customTestCasesPath = Path.Combine(levelPath, "customTests.json");
        CustomTestCasesSerialise serialisedTests = new CustomTestCasesSerialise(customTestCases, levelData.levelType);

        File.WriteAllTextAsync(customTestCasesPath, JsonUtility.ToJson(serialisedTests));
    }
}
