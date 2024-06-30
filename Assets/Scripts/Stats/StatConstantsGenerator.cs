using RPGGame.Stats;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class StatConstantsGenerator
{
    private const string AssetPath = "Assets/Scripts/Stats/StatConstants.cs";

    // This function generate automatically conts value from stats which are added to the stats container.
    // This helps to access the stats of the player simply by callling hero.Stats.GetAttribbute( ``StatConstans.Vitality`` ) 
    // Otherwise we had to give the StatType scriptable object to all classes which want to access stats of the player.

    public static void CreateScript(StatType[] statTypes)
    {
#if UNITY_EDITOR
        List<string> _statNames = new List<string>();
        for (int i = 0; i < statTypes.Length; i++)
        {
            if (_statNames.Contains(statTypes[i].Name))
            {
                Debug.LogError($"Stat Type {statTypes[i].Name} is added multiple times to Stat Container. Remove it and generate the stats again");
                return;
            }
            _statNames.Add(statTypes[i].Name);
        }

        using StreamWriter outfile = new StreamWriter(AssetPath);
        outfile.WriteLine("namespace RPGGame.Stats");
        outfile.WriteLine("{");
        outfile.WriteLine("    // This script is generated automatically from StatConstantGenerator which loops through all stats and adds them as constants.");
        outfile.WriteLine("    public static class StatConstants");
        outfile.WriteLine("    {");
        for (int i = 0; i < statTypes.Length; i++)
        {
            outfile.WriteLine($"        public const string {statTypes[i].Name}= \"{statTypes[i].ID}\";");
        }
        outfile.WriteLine("    }");
        outfile.WriteLine("}");

        AssetDatabase.Refresh();
#endif
    }
}
