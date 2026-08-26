using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct CustomTestCasesSerialise
{
    StandardTestCase.StandardTestCaseSerialise[] standardTestCases;
    SequentialTestCase.SequentialTestCaseSerialise[] sequentialTestCases;

    public CustomTestCasesSerialise(List<TestCase> testCases, LevelType levelType)
    {
        if (levelType == LevelType.standard)
        {
            standardTestCases = testCases.Select(s => (s as StandardTestCase).ToSerialised()).ToArray();
            sequentialTestCases = new SequentialTestCase.SequentialTestCaseSerialise[]{};
        }
        else if (levelType == LevelType.sequential)
        {
            standardTestCases = new StandardTestCase.StandardTestCaseSerialise[]{};
            sequentialTestCases = testCases.Select(s => (s as SequentialTestCase).ToSerialised()).ToArray();
        }
        else
        {
            standardTestCases = new StandardTestCase.StandardTestCaseSerialise[]{};
            sequentialTestCases = new SequentialTestCase.SequentialTestCaseSerialise[]{};
        }
    }

    public List<TestCase> GetTestCases(LevelType levelType)
    {
        if (levelType == LevelType.standard)
        {
            return standardTestCases.Select(s => new StandardTestCase(s)).ToList<TestCase>();
        }
        else if (levelType == LevelType.sequential)
        {
            return sequentialTestCases.Select(s => new SequentialTestCase(s)).ToList<TestCase>();
        }
        else
        {
            return new List<TestCase>();
        }
    }
}
