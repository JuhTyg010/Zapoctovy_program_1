using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public struct Combo
{
    public int main;
    public int[] minor;
    public Combo(int main, int[] minor)
    {
        this.main = main;
        this.minor = minor;
    }
}

public  class CalculateBestMatchFleet
{
    private const int SPAWNERS_COUNT = 6;
    
    //maybe in some database later who knows
    public static readonly Combo[] Combos = new Combo[]
    {
        new Combo(2, new []{2, 2}),
        new Combo(1, new []{3, 3}),
        new Combo(0, new []{6, 6}),
        new Combo(4, null)
    };
    
    
    public static Stack<Helper.ShipBaseParams> CreateFleet(List<Helper.ShipBaseParams> fleet, int combo, float difficulty)
    {
        Stack<Helper.ShipBaseParams> ships = new Stack<Helper.ShipBaseParams>();
        bool hasMinors = false;
        if (Combos[combo].main != 0)
        {
            foreach (var ship in fleet)
            {
                if (ShipIsMain(ship, difficulty / Combos[combo].main)) 
                {
                    if (Combos[combo].minor != null)
                    {
                        for (int i = 0; i < Combos[combo].minor.Length; i++)
                        {
                            int index = FindMinors(fleet, difficulty - (ship.difficulty * Combos[combo].main), combo);
                            if (index != -1)
                            {
                                for (int j = 0; j < Combos[combo].minor[i]; j++)
                                {
                                    ships.Push(fleet[index]);
                                }
                                hasMinors = true;
                            }
                            else break;
                        }
                    }
                    if (hasMinors)
                    {
                        for (int i = 0; i < Combos[combo].main; i++)
                        {
                            ships.Push(ship);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < Combos[combo].minor.Length; i++)
            {
                int index = FindMinors(fleet, difficulty, combo);
                if (index != -1)
                {
                    for (int j = 0; j < Combos[combo].minor[i]; j++)
                    {
                        ships.Push(fleet[index]);
                    }
                    
                }
                else break;
            }
        }
        return ships;
    }
    
    private static bool ShipIsMain(Helper.ShipBaseParams ship, float difficulty)
    {
        return SinglePowerIndex(ship, 1) >= difficulty;
    }
    
    private static bool ShipIsMinor(Helper.ShipBaseParams ship, float difficulty)
    {
        return SinglePowerIndex(ship, SPAWNERS_COUNT ) < (difficulty / 2);
    }
    
    private static int FindMinors(List<Helper.ShipBaseParams> fleet, float difficulty, int combo)
    {
        int index = -1;
        float best = 1000;
        for (int i = 0; i < fleet.Count; i++)
        {
            if (ShipIsMinor(fleet[i], difficulty / Combos[combo].minor.Length))
            {
                float powerIndex = SinglePowerIndex(fleet[i], SPAWNERS_COUNT);
                if (powerIndex > best && powerIndex < difficulty)
                {
                    best = powerIndex;
                    index = i;
                }
            }
        }
        return index;
    }

    private static float SinglePowerIndex(Helper.ShipBaseParams ship, int count)
    {
        return (ship.difficulty * count)  / (ship.spawnTime * Mathf.Ceil((float) count / SPAWNERS_COUNT));
    }
    
}
