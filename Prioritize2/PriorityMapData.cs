﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Prioritize2
{
    public class PriorityMapData : MapComponent
    {
        public class MapPriorityCache
        {
            public List<Thing> ThingCache = new List<Thing>();
            public bool IsDirty = false;
        }

        private int[] priorityGrid;

        public MapPriorityCache RenderCache = new MapPriorityCache();

        public PriorityMapData(Map map) : base(map)
        {
            InitGrid();
        }

        private void InitGrid()
        {
            priorityGrid = new int[map.cellIndices.NumGridCells];
        }

        public int GetPriorityAt(IntVec3 cell)
        {
            int index = map.cellIndices.CellToIndex(cell);
            if (index >= priorityGrid.Length || index < 0)
            {
                Log.ErrorOnce("PriorityMapGrid : GetPriorityAt with invalid cell (out of range).", "P2_PMGPA".GetHashCode());
                return 0;
            }
            return priorityGrid[index];
        }

        public void SetPriorityAt(IntVec3 cell, int pri)
        {
            int index = map.cellIndices.CellToIndex(cell);
            if (index >= priorityGrid.Length || index < 0)
            {
                Log.ErrorOnce("PriorityMapGrid : SetPriorityAt with invalid cell (out of range).", "P2_PMSPA".GetHashCode());
                return;
            }
            priorityGrid[index] = pri;
        }

        public override void MapRemoved()
        {
            GenPriorityMod.MapRemoved(map);
        }

        public override void ExposeData()
        {
            byte[] arr = null;

            if (Scribe.mode == LoadSaveMode.Saving)
            {
                if (priorityGrid == null || priorityGrid.Length != map.cellIndices.NumGridCells)
                {
                    Log.Error("PriorityMapGrid : Invalid priorityGrid. fixing..");
                    InitGrid();
                }
                arr = DataSerializeUtility.SerializeInt(priorityGrid);
            }
            DataExposeUtility.ByteArray(ref arr, "priorityGrid");
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                priorityGrid = DataSerializeUtility.DeserializeInt(arr);
            }
        }
    }
}