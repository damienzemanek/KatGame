using System;
using AYellowpaper.SerializedCollections;
using DesignPatterns.CreationalPatterns;
using UnityEngine;

public class PersistentConfiguration : Singleton<PersistentConfiguration>
{
    public SerializedDictionary<string, Player> players = new();
        
    [Serializable]
    public class Player
    {
        public string playerName;
        public Material playerMaterial;
    }
}
