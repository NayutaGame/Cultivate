
using System.Collections.Generic;
using System.Linq;

namespace CLLibrary
{
    public static class WeightedRandom
    {
        public static void SelectWeightedIndex(out int index, List<int> weights)
        {
            if (weights == null || weights.Count <= 0)
            {
                index = -1;
                return;
            }

            int totalWeight = weights.Sum();

            int randomWeight = UnityEngine.Random.Range(0, totalWeight);
            int currentWeight = 0;

            for (int i = 0; i < weights.Count; i++)
            {
                currentWeight += weights[i];
                if (randomWeight < currentWeight)
                {
                    index = i;
                    return;
                }
            }

            index = -1;
        }
    }
}