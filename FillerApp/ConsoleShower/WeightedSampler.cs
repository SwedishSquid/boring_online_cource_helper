using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShower
{
    public class WeightedSampler<T>
    {
        private readonly T[] values;
        private readonly int[] weights;
        private readonly int[] penalties;

        private readonly List<int> shuffledIndexList;

        private readonly int maxWeight = 1024;

        private readonly int maxPenalty = 10;

        private readonly int minWeight = 1;

        private int? sampledIndex = null;

        private static Random rng = new Random();

        public WeightedSampler(IEnumerable<T> values, IEnumerable<int>? initialWeights, int defaultWeight=16)
        {
            this.values = values.ToArray();
            weights = adjustWeights(initialWeights, this.values.Length, defaultWeight);
            penalties = new int[this.values.Length];
            shuffledIndexList = new List<int>();
            for (int i = 0; i < this.values.Length; i++)
            {
                shuffledIndexList.Add(i);
            }
        }

        private int[] adjustWeights(IEnumerable<int>? initialWeights, int requiredAmount, int defaultWeight)
        {
            var resultWeights = new int[requiredAmount];
            var init = new int[0];
            if (initialWeights != null)
            {
                init = initialWeights.ToArray();
            }
            for (int i = 0; i < requiredAmount; i++)
            {
                if (init.Length > i)
                {
                    resultWeights[i] = init[i];
                }
                else
                {
                    resultWeights[i] = defaultWeight;
                }
            }
            return resultWeights;
        }

        public int[] GetWeights() => weights;

        public T Sample(bool? increaseWeight)
        {
            if (increaseWeight is not null && sampledIndex is not null)
            {
                if (increaseWeight.Value)
                {
                    IncreaseWeight(sampledIndex.Value);
                }
                else
                {
                    DecreaseWeight(sampledIndex.Value);
                }
            }

            for (int i = 0; i < penalties.Length; i++)
            {
                if (penalties[i] > 0)
                {
                    penalties[i]--;
                }
            }

            if (sampledIndex.HasValue)
            {
                penalties[sampledIndex.Value] = maxPenalty;
            }

            var totalWeight = 0;
            for (int i = 0; i < values.Length; i++)
            {
                totalWeight += CalcWeightAt(i);
            }

            Shuffle(shuffledIndexList);

            var threshold = rng.Next(totalWeight);

            var currentSum = 0;

            for (int i = 0; i < values.Length; i++)
            {
                var index = shuffledIndexList[i];
                currentSum += weights[index];
                if (currentSum >= threshold)
                {
                    sampledIndex = index;
                    return values[index];
                }
            }

            sampledIndex = values.Length - 1;
            return values[values.Length - 1];
        }

        private int CalcWeightAt(int index)
        {
            var currentWeight = weights[index];
            var currentPenalty = penalties[index];
            while (currentPenalty > 0)
            {
                if (currentWeight <= minWeight)
                {
                    break;
                }
                currentWeight /= 2;
                currentPenalty -= 1;
            }
            return currentWeight;
        }

        private void IncreaseWeight(int index)
        {
            if (weights[index] * 2 <= maxWeight)
            {
                weights[index] *= 2;
            }
            else
            {
                weights[index] = maxWeight;
            }
        }

        private void DecreaseWeight(int index)
        {
            if (weights[index] >= minWeight * 2)
            {
                weights[index] /= 2;
            }
            else
            {
                weights[index] = minWeight;
            }
        }

        public static void Shuffle<Type>(IList<Type> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
