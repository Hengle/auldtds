using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMaster
{
    public class DungeonMaster
    {
        public int Roll(int sides)
        {
            return Random.Range(1, sides);
        }

    }
}

    
