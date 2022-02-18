using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricateTerrain : MapFabricationPass
{
    public override void Execute(MapData data, GameObject root)
    {
        // How do Fabrication Passes get their settings..
        // .. is it on the pass or are they referencing some database?
        // does that mean FabricationPasses are linked to Managers?
        // probably, yes. They need to make REAL GAME OBJECTS IN THE REAL GAME WORLD.
        // of course they will need to interact with Managers.
        // this means that MapFabrication should only take place in Play Mode?
        // that would be easier...
    }
}
