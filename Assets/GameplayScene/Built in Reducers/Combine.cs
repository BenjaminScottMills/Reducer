using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : Reducer
{
    public Reducer earth;
    public Reducer fire;
    public Reducer plant;
    public Reducer water;
    public override ExecuteReducer Execute(ExecuteReducer black, ExecuteReducer white)
    {
        if (black == null && white == null)
        {
            return new ExecuteReducer(this);
        }
        else
        {
            if (black == null)
            {
                black = new ExecuteReducer(nullReducer);
            }
            else if (white == null)
            {
                white = new ExecuteReducer(nullReducer);
            }
        }

        if (black.selfRed.id == (int)SpecialReducers.nullRed)
        {
            return white;
        }
        else if (black.selfRed.id <= (int)SpecialReducers.water)
        {
            switch (white.selfRed.id)
            {
                case (int)SpecialReducers.nullRed:
                    return black;
                case (int)SpecialReducers.earth:
                    switch (black.selfRed.id)
                    {
                        case (int)SpecialReducers.earth:
                            return new ExecuteReducer(fire);
                        case (int)SpecialReducers.fire:
                            return new ExecuteReducer(earth);
                        case (int)SpecialReducers.plant:
                            return new ExecuteReducer(plant);
                        case (int)SpecialReducers.water:
                            return new ExecuteReducer(plant);
                        default:
                            throw new System.Exception("id messed up");
                    }
                case (int)SpecialReducers.fire:
                    switch (black.selfRed.id)
                    {
                        case (int)SpecialReducers.earth:
                            return new ExecuteReducer(earth);
                        case (int)SpecialReducers.fire:
                            return new ExecuteReducer(fire);
                        case (int)SpecialReducers.plant:
                            return new ExecuteReducer(earth);
                        case (int)SpecialReducers.water:
                            return new ExecuteReducer(nullReducer);
                        default:
                            throw new System.Exception("id messed up");
                    }
                case (int)SpecialReducers.plant:
                    switch (black.selfRed.id)
                    {
                        case (int)SpecialReducers.earth:
                            return new ExecuteReducer(plant);
                        case (int)SpecialReducers.fire:
                            return new ExecuteReducer(earth);
                        case (int)SpecialReducers.plant:
                            return new ExecuteReducer(nullReducer);
                        case (int)SpecialReducers.water:
                            return new ExecuteReducer(plant);
                        default:
                            throw new System.Exception("id messed up");
                    }
                case (int)SpecialReducers.water:
                    switch (black.selfRed.id)
                    {
                        case (int)SpecialReducers.earth:
                            return new ExecuteReducer(plant);
                        case (int)SpecialReducers.fire:
                            return new ExecuteReducer(nullReducer);
                        case (int)SpecialReducers.plant:
                            return new ExecuteReducer(plant);
                        case (int)SpecialReducers.water:
                            return new ExecuteReducer(water);
                        default:
                            throw new System.Exception("id messed up");
                    }
                default:
                    return black;
            }
        }
        else
        {
            if (white.selfRed.id == (int)SpecialReducers.nullRed)
            {
                return black;
            }
            else if (white.selfRed.id <= (int)SpecialReducers.water)
            {
                return white;
            }
            else
            {
                throw new System.Exception("combine 2 functions");
            }
        }
    }
}
