using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day15: BaseDayWithInput
    {
        struct Ingredient
        {
            public string Name;
            public int Capacity;
            public int Durability;
            public int Flavor;
            public int Texture;
            public int Calories;
        }
        readonly List<Ingredient> ingredients;
        readonly List<Ingredient> CapacityPositiveCookies;
        readonly List<Ingredient> DurabilityPositiveCookies;
        readonly List<Ingredient> FlavorPositiveCookies;
        readonly List<Ingredient> TexturePositiveCookies;
        public Day15()
        {
            CapacityPositiveCookies = [];
            DurabilityPositiveCookies = [];
            FlavorPositiveCookies = [];
            TexturePositiveCookies = [];
            ingredients = [];
            foreach (var line in _input)
            {
                var parts = line.Split(' ');
                ingredients.Add(new Ingredient
                {
                    Name = parts[0].TrimEnd(':'),
                    Capacity = int.Parse(parts[2].TrimEnd(',')),
                    Durability = int.Parse(parts[4].TrimEnd(',')),
                    Flavor = int.Parse(parts[6].TrimEnd(',')),
                    Texture = int.Parse(parts[8].TrimEnd(',')),
                    Calories = int.Parse(parts[10])
                });
            }
            foreach (var ingredient in ingredients)
            {
                if (ingredient.Capacity > 0)
                    CapacityPositiveCookies.Add(ingredient);
                if (ingredient.Durability > 0)
                    DurabilityPositiveCookies.Add(ingredient);
                if (ingredient.Flavor > 0)
                    FlavorPositiveCookies.Add(ingredient);
                if (ingredient.Texture > 0)
                    TexturePositiveCookies.Add(ingredient);
            }
        }
        Dictionary<(int, int, int, int, int, int?), int> cache = [];

        public int GetBestCookieP1((int, int, int, int, int, int?) key)
        {
            if (!cache.ContainsKey(key))
                cache.Add(key, GetBestCookieP1(key.Item1, key.Item2, key.Item3, key.Item4, key.Item5, key.Item6));
            return cache[key];
        }
        public int GetBestCookieP1(int ingredientCount,int Capacity,int Duability,int Flavor, int Texture, int? Calories = null)
        {
            if (Calories.HasValue && Calories > 500)
                return 0;
            if (ingredientCount>100)
                return 0;
            if (ingredientCount == 100)
            {
                if(Calories.HasValue && Calories != 500)
                    return 0;
                if (Capacity > 0 && Duability > 0 && Flavor > 0 && Texture > 0)
                    return Capacity * Duability * Flavor * Texture;
                return 0;
            }
            HashSet<Ingredient> haveToAdd = [];
            if (Capacity <= 0) CapacityPositiveCookies.ForEach(x => haveToAdd.Add(x));
            if (Duability <= 0) DurabilityPositiveCookies.ForEach(x => haveToAdd.Add(x));
            if (Flavor <= 0) FlavorPositiveCookies.ForEach(x => haveToAdd.Add(x));
            if (Texture <= 0) TexturePositiveCookies.ForEach(x => haveToAdd.Add(x));
            if (haveToAdd.Count == 0)
            {
                int max = 0;
                foreach(var ingredient in ingredients)
                {
                    var key = (ingredientCount + 1,
                        Capacity + ingredient.Capacity,
                        Duability + ingredient.Durability,
                        Flavor + ingredient.Flavor,
                        Texture + ingredient.Texture,
                        Calories.HasValue ? Calories + ingredient.Calories : null);
                    int result = GetBestCookieP1(key);
                    if (result > max)
                        max = result;
                }
                return max;
            }
            foreach (var ingredient in haveToAdd)
            {
                ingredientCount++;
                Capacity += ingredient.Capacity;
                Duability += ingredient.Durability;
                Flavor += ingredient.Flavor;
                Texture += ingredient.Texture;
                if (Calories.HasValue)
                    Calories += ingredient.Calories;
            }
            return GetBestCookieP1((ingredientCount, Capacity, Duability, Flavor, Texture, Calories));
        }
        public override ValueTask<string> Solve_1() => new($"{GetBestCookieP1((0, 0, 0, 0, 0, null))}");
        public override ValueTask<string> Solve_2() => new($"{GetBestCookieP1((0, 0, 0, 0, 0, 0))}");
    }
}
