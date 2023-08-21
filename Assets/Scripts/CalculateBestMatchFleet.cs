using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public  class CalculateBestMatchFleet
{
    private const int SPAWNERS_COUNT = 6;
    
   
    
    public static Stack<Helper.ShipBaseParams> CreateFleet(List<Helper.ShipBaseParams> fleet, float difficulty, bool isBoss)
    {
        Stack<Helper.ShipBaseParams> currentFleet = new Stack<Helper.ShipBaseParams>();
        if (isBoss)
        {
            List<(int, float)> coefficient = new List<(int, float)>();
            foreach (var ship in fleet)
            {
                int count = MaxInBurst(ship, difficulty);
                float differ = difficulty - count * ship.difficulty;
                if (count > 6)
                    differ = difficulty;
                else if (count < 3)
                    differ *= 0.5f;
                coefficient.Add((ship.shipID, differ));
            }

            int idealShipID = FindBestCoefficient(coefficient);
            Helper.ShipBaseParams idealShip = fleet.Find(x => x.shipID == idealShipID);
            int idealCount = MaxInBurst(idealShip, difficulty);
            for (int i = 0; i < idealCount; i++)
                currentFleet.Push(idealShip);
        }
        else
        {
            List<(int, float)> coefficient = new List<(int, float)>();
            foreach (var ship in fleet)
            {
                int count = MaxInBurst(ship, difficulty);
                float differ = difficulty - count * ship.difficulty;
                if (count % 6 == 0)
                    differ *= 0.5f;
                else if (count % 3 == 0)
                    differ *= 0.66f;
                else if (count % 2 == 0)
                    differ *= 0.75f;
                if (count > 24)
                    differ += difficulty;
                else if (count < 5)
                    differ += difficulty;
                coefficient.Add((ship.shipID, differ));
            }

            int idealShipID = FindBestCoefficient(coefficient);
            Helper.ShipBaseParams idealShip = fleet.Find(x => x.shipID == idealShipID);
            int idealCount = MaxInBurst(idealShip, difficulty);
            for (int i = 0; i < idealCount; i++)
                currentFleet.Push(idealShip);
        }

        return currentFleet;
    }


    private static int MaxInBurst(Helper.ShipBaseParams ship, float difficulty)
    {
        int count;
        for (count = 0; count * ship.difficulty <= difficulty; count++)
        {
        }
        return count - 1;
    }
    
    private static int FindBestCoefficient(List<(int, float)> coefficient)
    {
        float min = coefficient[0].Item2;
        int index = 0;
        for (int i = 1; i < coefficient.Count; i++)
        {
            if (coefficient[i].Item2 < min)
            {
                min = coefficient[i].Item2;
                index = i;
            }
        }

        return coefficient[index].Item1;
    }
    
    //Maybe will be useful sometime in future
    private static float SinglePowerIndex(Helper.ShipBaseParams ship, int count)
    {
        return (ship.difficulty * count) / (ship.spawnTime * Mathf.Ceil((float) count / SPAWNERS_COUNT));
    }
}
