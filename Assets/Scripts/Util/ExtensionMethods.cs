using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {
    private static Random random = new Random();

    public static void Shuffle(this IList list) {
        int n = list.Count;

        while (n > 1) {
            n--;

            int k = random.Next(n + 1);
            Object value = list[k];

            list[k] = list[n];
            list[n] = value;
        }
    }
}
