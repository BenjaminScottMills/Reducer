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
    Solution groundTruthSolution;
    public GameObject dummySolutionPrefab;
    public Solution mainSolution;
    public LevelType Initialise(string levelPathArg)
    {
        groundTruthSolution = Instantiate(dummySolutionPrefab, transform).GetComponent<Solution>();
        groundTruthSolution.CopyFixedReducers(mainSolution);
        groundTruthSolution.mouseNode = mainSolution.mouseNode;
        testCasesList.groundTruthSolution = groundTruthSolution;

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

        var fixedTestCases = levelData.GetTestCases();
        for (int i = 0; i < fixedTestCases.Count; i++)
        {
            testCasesList.AddTestCase(fixedTestCases[i], false, i);
        }

        for (int i = 0; i < customTestCases.Count; i++)
        {
            testCasesList.AddTestCase(customTestCases[i], true, i);
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
