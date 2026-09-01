using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RequirementsTab : MonoBehaviour
{
    string levelPath;
    LevelData levelData;
    List<TestCase> customTestCases;
    public Text requirementsText;
    public TestCasesList testCasesList;
    public Solution groundTruthSolution;
    public LevelType Initialise(string levelPathArg)
    {
        levelPath = levelPathArg;
        string levelDataPath = Path.Combine(levelPath, "levelData.json");
        levelData = LevelData.MakeLevelData(File.ReadAllText(levelDataPath), groundTruthSolution);
        requirementsText.text = levelData.requirementsDescription;
        string customTestCasesPath = Path.Combine(levelPath, "customTests.json");
        if (File.Exists(customTestCasesPath))
        {
            customTestCases = JsonUtility.FromJson<CustomTestCasesSerialise>(File.ReadAllText(levelDataPath)).GetTestCases(levelData.levelType);
        }
        else
        {
            customTestCases = new();
        }

        foreach (TestCase tc in levelData.GetTestCases())
        {
            testCasesList.AddTestCase(tc, false);
        }

        foreach (TestCase tc in customTestCases)
        {
            testCasesList.AddTestCase(tc, true);
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
