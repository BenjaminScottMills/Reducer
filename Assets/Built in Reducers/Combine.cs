using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : Reducer
{
    public Reducer earth;
    public Reducer fire;
    public Reducer plant;
    public Reducer water;
    public override Reducer ExecuteFast(Reducer black, Reducer white)
    {
        if (black == null && white == null)
        {
            return this;
        }
        if (black.id == (int)SpecialReducers.nullRed)
        {
            return white;
        }
        else if (black.id <= (int)SpecialReducers.water)
        {
            switch (white.id)
            {
                case (int)SpecialReducers.nullRed:
                    return black;
                case (int)SpecialReducers.earth:
                    switch (black.id)
                    {
                        case (int)SpecialReducers.earth:
                            return fire;
                        case (int)SpecialReducers.fire:
                            return earth;
                        case (int)SpecialReducers.plant:
                            return plant;
                        case (int)SpecialReducers.water:
                            return plant;
                        default:
                            throw new System.Exception("id messed up");
                    }
                case (int)SpecialReducers.fire:
                    switch (black.id)
                    {
                        case (int)SpecialReducers.earth:
                            return earth;
                        case (int)SpecialReducers.fire:
                            return fire;
                        case (int)SpecialReducers.plant:
                            return earth;
                        case (int)SpecialReducers.water:
                            return nullReducer;
                        default:
                            throw new System.Exception("id messed up");
                    }
                case (int)SpecialReducers.plant:
                    switch (black.id)
                    {
                        case (int)SpecialReducers.earth:
                            return plant;
                        case (int)SpecialReducers.fire:
                            return earth;
                        case (int)SpecialReducers.plant:
                            return nullReducer;
                        case (int)SpecialReducers.water:
                            return plant;
                        default:
                            throw new System.Exception("id messed up");
                    }
                case (int)SpecialReducers.water:
                    switch (black.id)
                    {
                        case (int)SpecialReducers.earth:
                            return plant;
                        case (int)SpecialReducers.fire:
                            return nullReducer;
                        case (int)SpecialReducers.plant:
                            return plant;
                        case (int)SpecialReducers.water:
                            return water;
                        default:
                            throw new System.Exception("id messed up");
                    }
                default:
                    throw new System.Exception("combine 2 functions");
            }
        }
        else
        {
            if (white.id == (int)SpecialReducers.nullRed)
            {
                return black;
            }
            else if (white.id <= (int)SpecialReducers.water)
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
