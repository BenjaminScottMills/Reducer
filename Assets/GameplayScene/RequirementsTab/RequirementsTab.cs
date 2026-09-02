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
    bool customTestCasesDirtied;
    public Text requirementsText;
    public TestCasesList testCasesList;
    Solution groundTruthSolution;
    public GameObject dummySolutionPrefab;
    public Solution mainSolution;
    public LevelType Initialise(string levelPathArg)
    {
        customTestCasesDirtied = false;
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
            customTestCases = JsonUtility.FromJson<CustomTestCasesSerialise>(File.ReadAllText(customTestCasesPath)).GetTestCases(levelData.levelType);
        }
        else
        {
            customTestCases = new();
        }

        var fixedTestCases = levelData.GetTestCases();
        int privateTestNumber = 1;
        int publicTestNumber = 1;
        foreach (TestCase tc in fixedTestCases)
        {
            int tcNumber;
            if (tc.isPrivate)
            {
                tcNumber = privateTestNumber;
                privateTestNumber++;
            }
            else
            {
                tcNumber = publicTestNumber;
                publicTestNumber++;
            }
            testCasesList.AddTestCase(tc, false, tcNumber);
        }

        for (int i = 0; i < customTestCases.Count; i++)
        {
            testCasesList.AddTestCase(customTestCases[i], true, i + 1);
        }

        return levelData.levelType;
    }

    public void WriteCustomTestCases()
    {
        if (!customTestCasesDirtied) return;
        customTestCasesDirtied = false;
        string customTestCasesPath = Path.Combine(levelPath, "customTests.json");
        CustomTestCasesSerialise serialisedTests = new CustomTestCasesSerialise(customTestCases, levelData.levelType);
        Debug.Log(serialisedTests.standardTestCases.Length);
        File.WriteAllTextAsync(customTestCasesPath, JsonUtility.ToJson(serialisedTests));
    }

    public void RemoveTestCase(TestCase removeTestCase)
    {
        customTestCases.Remove(removeTestCase);
        customTestCasesDirtied = true;
    }

    public void CreateCustomTestCase()
    {
        TestCase newTestCase = levelData.CreateNewTestCase();
        customTestCases.Add(newTestCase);
        customTestCasesDirtied = true;

        testCasesList.AddTestCase(newTestCase, true, customTestCases.Count);
    }
}
