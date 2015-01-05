using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {
    private static System.Random random = new System.Random();

    public static void Shuffle(this IList list) {
        int n = list.Count;

        while (n > 1) {
            n--;

            int k = random.Next(n + 1);
            System.Object value = list[k];

            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool EqualsIgnoreCase(this string text, string compareTo) {
        return text.Equals(text, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool ContainsName(this Dictionary<NetworkPlayer, PlayerInfo>.ValueCollection list, string name) {
        HashSet<string> names = new HashSet<string>();

        foreach (PlayerInfo playerInfo in list) {
            if (playerInfo.name != null) {
                names.Add(playerInfo.name.ToLower());
            }
        }

        return names.Contains(name.ToLower());
    }
}
