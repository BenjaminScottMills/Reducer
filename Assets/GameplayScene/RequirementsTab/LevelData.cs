using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LevelType {standard, sequential, definition, tutorial};
public enum SchemaType {primitiveOrFunction, primitive, function, boolean, natNumber, finiteList, infiniteList};
public enum ReducerValue {nullRed, fire, earth, plant, water, combine, testReducer}; // testReducer is the special reducer which outputs its child, which has an outerBlack and outerWhite node.

public abstract class LevelData
{
    public string requirementsDescription;
    public LevelType levelType; // corresponds to enum LevelType
    public Solution groundTruthSolution;
    public ReducerSchema outputSchema;

    protected LevelData(LevelDataSerialise serialisedLevelData, Solution groundTruthSolutionArg)
    {
        requirementsDescription = serialisedLevelData.requirementsDescription;
        levelType = serialisedLevelData.levelType;
        groundTruthSolution = groundTruthSolutionArg;
        Debug.Log("Remove this if statement check once we have this assigned");
        // groundTruthSolution.LoadFromSerialisedForImporting(serialisedLevelData.groundTruthSolution);
        outputSchema = serialisedLevelData.outputSchema.ReducerSchemaFromSerialised();
    }

    public static LevelData MakeLevelData(string serialisedText, Solution groundTruthSolutionObject)
    {
        LevelDataSerialise serialisedLevelData = JsonUtility.FromJson<LevelDataSerialise>(serialisedText);

        switch (serialisedLevelData.levelType)
        {
            case LevelType.standard:
                return new StandardLevelData(serialisedLevelData, groundTruthSolutionObject);
            case LevelType.sequential:
                return new SequentialLevelData(serialisedLevelData, groundTruthSolutionObject);
            default:
                Debug.Log("Level type is unimplemented");
                return null;
        }
    }

    public abstract List<TestCase> GetTestCases();


    [System.Serializable]
    public struct LevelDataSerialise
    {
        public string requirementsDescription;
        public LevelType levelType;
        public SolutionSerialise groundTruthSolution;
        public SchemaSerialise blackInputSchema; // for sequential, black is input
        public SchemaSerialise whiteInputSchema;
        public SchemaSerialise outputSchema;
        public StandardTestCase.StandardTestCaseSerialise[] standardtestCases;
        public SequentialTestCase.SequentialTestCaseSerialise[] sequentialTestCases;


        [System.Serializable]
        public struct SchemaSerialise
        {
            public SchemaSerialiseComponent[] components;

            public ReducerSchema ReducerSchemaFromSerialised()
            {
                return ReducerSchemaFromComponents(0, out _);
            }

            ReducerSchema ReducerSchemaFromComponents(int idx, out int outIdxValue)
            {
                SchemaSerialiseComponent targetComponent = components[idx];
                idx += 1;
                ReducerSchema outputSchema;

                switch (targetComponent.type)
                {
                    case SchemaType.finiteList:
                        FiniteListReducerSchema finiteListOutputSchema = new FiniteListReducerSchema();
                        for (int i = 0; i < targetComponent.finiteListLength; i++)
                        {
                            finiteListOutputSchema.childSchemas.Add(ReducerSchemaFromComponents(idx, out idx));
                        }

                        outputSchema = finiteListOutputSchema;
                        break;
                    case SchemaType.infiniteList:
                        outputSchema = new InfiniteListReducerSchema(ReducerSchemaFromComponents(idx, out idx));
                        break;
                    default:
                        outputSchema = new SimpleReducerSchema(targetComponent.primitiveValueSet);
                        break;
                }
                outputSchema.type = targetComponent.type;

                outIdxValue = idx;
                return outputSchema;
            }


            [System.Serializable]
            public struct SchemaSerialiseComponent
            {
                public SchemaType type;
                public Reducer.SpecialReducers[] primitiveValueSet; // applies if type is primitive. If so, this is set of values allowed.
                public int finiteListLength;
            }
        }
    }
}

public class StandardLevelData : LevelData
{
    public ReducerSchema blackInputSchema;
    public ReducerSchema whiteInputSchema;
    public List<StandardTestCase> testCases;

    public StandardLevelData(LevelDataSerialise serialisedLevelData, Solution groundTruthSolutionArg) : base(serialisedLevelData, groundTruthSolutionArg)
    {
        blackInputSchema = serialisedLevelData.blackInputSchema.ReducerSchemaFromSerialised();
        whiteInputSchema = serialisedLevelData.whiteInputSchema.ReducerSchemaFromSerialised();
        testCases = serialisedLevelData.standardtestCases.Select(s => new StandardTestCase(s)).ToList();
    }

    public override List<TestCase> GetTestCases()
    {
        return new List<TestCase>(testCases);
    }
}

public class SequentialLevelData : LevelData
{
    public ReducerSchema inputSchema;
    public List<SequentialTestCase> testCases;

    public SequentialLevelData(LevelDataSerialise serialisedLevelData, Solution groundTruthSolutionArg) : base(serialisedLevelData, groundTruthSolutionArg)
    {
        inputSchema = serialisedLevelData.blackInputSchema.ReducerSchemaFromSerialised();
        testCases = serialisedLevelData.sequentialTestCases.Select(s => new SequentialTestCase(s)).ToList();
    }

    public override List<TestCase> GetTestCases()
    {
        return new List<TestCase>(testCases);
    }
}


// ReducerSchema classes
public abstract class ReducerSchema
{
    public SchemaType type;
}

public class SimpleReducerSchema : ReducerSchema
{
    public Reducer.SpecialReducers[] primitiveValueSet; // only applies for primitive or primitiveOrFunction. If empty, all are allowed

    public SimpleReducerSchema(Reducer.SpecialReducers[] primitiveValueSetArg)
    {
        primitiveValueSet = primitiveValueSetArg;
    }
}

public class FiniteListReducerSchema : ReducerSchema
{
    public List<ReducerSchema> childSchemas;
    public FiniteListReducerSchema()
    {
        childSchemas = new();
    }
}

public class InfiniteListReducerSchema : ReducerSchema
{
    public ReducerSchema childSchema;
    public InfiniteListReducerSchema(ReducerSchema childSchemaArg)
    {
        childSchema = childSchemaArg;
    }
}


// TestCase classes
public abstract class TestCase
{
    public bool isPrivate;

    protected TestCase(bool isPrivateArg)
    {
        isPrivate = isPrivateArg;
    }
}

public class StandardTestCase : TestCase
{
    public TestCaseInput blackInput;
    public TestCaseInput whiteInput;

    public StandardTestCase(StandardTestCaseSerialise serialised) : base(serialised.isPrivate)
    {
        blackInput = serialised.blackInput.TestCaseInputFromSerialised();
        whiteInput = serialised.whiteInput.TestCaseInputFromSerialised();
    }

    public StandardTestCaseSerialise ToSerialised()
    {
        return new StandardTestCaseSerialise
        {
            isPrivate = isPrivate,
            blackInput = blackInput.ToSerialised(),
            whiteInput = whiteInput.ToSerialised(),
        };
    }


    [System.Serializable]
    public struct StandardTestCaseSerialise
    {
        public bool isPrivate;
        public TestCaseInput.TestCaseInputSerialise blackInput;
        public TestCaseInput.TestCaseInputSerialise whiteInput;
    }
}

public class SequentialTestCase : TestCase
{
    public List<TestCaseInput> inputs;

    public SequentialTestCase(SequentialTestCaseSerialise serialised) : base(serialised.isPrivate)
    {
        inputs = serialised.inputs.Select(s => s.TestCaseInputFromSerialised()).ToList();
    }

    public SequentialTestCaseSerialise ToSerialised()
    {
        return new SequentialTestCaseSerialise
        {
            isPrivate = isPrivate,
            inputs = inputs.Select(i => i.ToSerialised()).ToArray(),
        };
    }


    [System.Serializable]
    public struct SequentialTestCaseSerialise
    {
        public bool isPrivate;
        public TestCaseInput.TestCaseInputSerialise[] inputs;
    }
}


// TestCaseInput stuff
public abstract class TestCaseInput
{
    public SchemaType type;

    public TestCaseInputSerialise ToSerialised()
    {
        List<TestCaseComponent> componentsList = new List<TestCaseComponent>();
        UpdateComponentsList(componentsList);

        return new TestCaseInputSerialise{components = componentsList.ToArray()};
    }

    public abstract void UpdateComponentsList(List<TestCaseComponent> componentsList);


    [System.Serializable]
    public struct TestCaseInputSerialise
    {
        public TestCaseComponent[] components;

        public TestCaseInput TestCaseInputFromSerialised()
        {
            return TestCaseInputFromComponents(0, out _);
        }

        TestCaseInput TestCaseInputFromComponents(int idx, out int outIdxValue)
        {
            TestCaseComponent targetComponent = components[idx];
            idx += 1;
            TestCaseInput testCaseOutput;

            switch (targetComponent.type)
            {
                case SchemaType.finiteList:
                case SchemaType.infiniteList:
                    ListTestCaseInput finiteListOutputSchema = new ListTestCaseInput();
                    for (int i = 0; i < targetComponent.listLength; i++)
                    {
                        finiteListOutputSchema.listValue.Add(TestCaseInputFromComponents(idx, out idx));
                    }

                    testCaseOutput = finiteListOutputSchema;
                    break;
                case SchemaType.natNumber:
                    testCaseOutput = new NumberTestCaseInput(targetComponent.numberValue);
                    break;
                case SchemaType.boolean:
                    testCaseOutput = new BooleanTestCaseInput(targetComponent.booleanValue);
                    break;
                default:
                    testCaseOutput = new SimpleReducerTestCaseInput(targetComponent.reducerValue);
                    break;
            }
            testCaseOutput.type = targetComponent.type;

            outIdxValue = idx;
            return testCaseOutput;
        }
    }


    [System.Serializable]
    public struct TestCaseComponent
    {
        public SchemaType type; // only use primitiveOrFunction, just treat primitive and function types as primitiveOrFunction.
        public ReducerValue reducerValue; // for primitive, function, and primitiveOrFunction. 
        public bool booleanValue;
        public int numberValue;
        public int listLength; // treat finiteList and infiniteList the same
    }
}

public class SimpleReducerTestCaseInput : TestCaseInput
{
    public ReducerValue reducerValue;

    public SimpleReducerTestCaseInput(ReducerValue reducerValueArg)
    {
        reducerValue = reducerValueArg;
    }

    public override void UpdateComponentsList(List<TestCaseComponent> componentsList)
    {
        componentsList.Add(
            new TestCaseComponent
            {
                type = type,
                reducerValue = reducerValue
            }
        );
    }
}

public class BooleanTestCaseInput : TestCaseInput
{
    public bool booleanValue;

    public BooleanTestCaseInput(bool booleanValueArg)
    {
        booleanValue = booleanValueArg;
    }

    public override void UpdateComponentsList(List<TestCaseComponent> componentsList)
    {
        componentsList.Add(
            new TestCaseComponent
            {
                type = type,
                booleanValue = booleanValue
            }
        );
    }
}

public class NumberTestCaseInput : TestCaseInput
{
    public int numberValue;

    public NumberTestCaseInput(int numberValueArg)
    {
        numberValue = numberValueArg;
    }

    public override void UpdateComponentsList(List<TestCaseComponent> componentsList)
    {
        componentsList.Add(
            new TestCaseComponent
            {
                type = type,
                numberValue = numberValue
            }
        );
    }
}

public class ListTestCaseInput : TestCaseInput
{
    public List<TestCaseInput> listValue;

    public ListTestCaseInput()
    {
        listValue = new();
    }

    public override void UpdateComponentsList(List<TestCaseComponent> componentsList)
    {
        componentsList.Add(
            new TestCaseComponent
            {
                type = type,
                listLength = listValue.Count
            }
        );

        foreach (TestCaseInput subInput in listValue)
        {
            subInput.UpdateComponentsList(componentsList);
        }
    }
}
