using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct CustomTestCasesSerialise
{
    StandardTestCase.StandardTestCaseSerialise[] standardTestCases;
    SequentialTestCase.SequentialTestCaseSerialise[] sequentialTestCases;

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
