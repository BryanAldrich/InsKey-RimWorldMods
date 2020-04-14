﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;

namespace Prioritize2
{
    public static class GenPriorityMod
    {
        //Just maybe..GetComponent() is slower than this?
        private static Dictionary<Map, PriorityMapData> PriorityMapDataCache = new Dictionary<Map, PriorityMapData>();

        public static PriorityMapData GetPriorityData(this Map map)
        {
            if (PriorityMapDataCache.TryGetValue(map, out PriorityMapData data))
            {
                return data;
            }
            else
            {
                var comp = map.GetComponent<PriorityMapData>();
                PriorityMapDataCache.Add(map, comp);

                return comp;
            }
        }

        //Remove from PriorityMapDataCache
        public static void MapRemoved(Map map)
        {
            PriorityMapDataCache.Remove(map);
        }

        public static Color GetPriorityColor(this int val)
        {
            float lerpAlpha = (MainMod.ModConfig.priorityMax - MainMod.ModConfig.priorityMin) / (val - MainMod.ModConfig.priorityMin);

            return Color.Lerp(MainMod.ModConfig.LowPriorityColor, MainMod.ModConfig.HighPriorityColor, lerpAlpha);
        }

        public static Color FromHex(uint hexColor)
        {
            uint r = (hexColor >> 24) & 0x000000ff;
            uint g = (hexColor >> 16) & 0x000000ff;
            uint b = (hexColor >> 8) & 0x000000ff;
            uint a = (hexColor >> 0) & 0x000000ff;

            return new Color(r, g, b, a);
        }
    }
}
