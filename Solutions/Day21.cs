using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day21: BaseDayWithInput
    {
        const string shopInput =
        """
        Weapons:    Cost  Damage  Armor
        Dagger        8     4       0
        Shortsword   10     5       0
        Warhammer    25     6       0
        Longsword    40     7       0
        Greataxe     74     8       0

        Armor:      Cost  Damage  Armor
        Leather      13     0       1
        Chainmail    31     0       2
        Splintmail   53     0       3
        Bandedmail   75     0       4
        Platemail   102     0       5

        Rings:      Cost  Damage  Armor
        Damage +1    25     1       0
        Damage +2    50     2       0
        Damage +3   100     3       0
        Defense +1   20     0       1
        Defense +2   40     0       2
        Defense +3   80     0       3
        """;
        enum ItemType
        {
            Weapon,
            Armor,
            Ring
    }
        struct Item
        {
            public ItemType Type;
            public string Name;
            public int Cost;
            public int Damage;
            public int Armor;
            public override readonly string ToString() =>
                $"{Type,-10} {Name,-10} \t {Cost,3}$ \t {Damage}⚔ \t {Armor}🛡";
        }
        class Tank(int hitPoints, int damage, int armor)
        {
            public int HitPoints { get; private set; } = hitPoints;
            public int Damage { get; private set; } = damage;
            public int Armor { get; private set; } = armor;
            public int TotalCost { get; private set; }
            public string? Equipped { get; private set; }
            private void Equip(Item item)
            {
                Equipped += item.Name;
                Damage += item.Damage;
                Armor += item.Armor;
                TotalCost += item.Cost;
            }
            public bool IsAlive(int incomingDamage)
            {
                HitPoints -= Math.Max(1, incomingDamage - Armor);
                return HitPoints > 0;
            }
            public static Tank MakeBoss((int hp,int dmg,int armor)stats) =>
                new(stats.hp, stats.dmg, stats.armor);
            public static Tank MakePlayer(int hp, List<Item> items)
            {
                var player = new Tank(hp, 0, 0);
                foreach (var item in items)
                    player.Equip(item);
                return player;
            }
            internal Tank Clone() => new(HitPoints, Damage, Armor);
        }
        private readonly List<Item> shop;
        private readonly (int hp, int damage, int armor) bossStats;
        public Day21()
        {
            shop = [];
            var shopCategories = shopInput.Split("\r\n\r\n");
            foreach (var shopCategory in shopCategories)
            {
                var shopRows = shopCategory.Split("\r\n");
                ItemType itemType = shopRows[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0] switch
                {
                    "Weapons:" => ItemType.Weapon,
                    "Armor:" => ItemType.Armor,
                    "Rings:" => ItemType.Ring,
                    _ => throw new Exception("Invalid shop category")
                };
                foreach (var shopRow in shopRows.Skip(1))
                {
                    var shopRowDetails = shopRow.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    shop.Add(new(){
                        Type = itemType,
                        Armor = int.Parse(shopRowDetails[^1]),
                        Damage = int.Parse(shopRowDetails[^2]),
                        Cost = int.Parse(shopRowDetails[^3]),
                        Name = string.Join('_', shopRowDetails[..^3])
                    });
                }
            }
            bossStats = (
                int.Parse(_input[0].Split(':')[1].Trim()),
                int.Parse(_input[1].Split(':')[1].Trim()),
                int.Parse(_input[2].Split(':')[1].Trim()));

        }
        private bool Battle(Tank playerLoadout)
        {
            var player = playerLoadout.Clone();
            var boss = Tank.MakeBoss(bossStats);
            while(boss.IsAlive(player.Damage))
                if (!player.IsAlive(boss.Damage))
                    return false;
            return true;
        }
        private List<Tank>? LoadoutCache=null;
        private List<Tank> GetPossiblePlayerLoadoutsOrderedByCost()
        {
            if(LoadoutCache is not null)
                return LoadoutCache;
            var weapons = shop.Where(item => item.Type == ItemType.Weapon).ToList();
            var armors = shop.Where(item => item.Type == ItemType.Armor).ToList();
            var rings = shop.Where(item => item.Type == ItemType.Ring).ToList();
            List<Tank> playerLoadouts = [];
            foreach (var weapon in weapons)
            {
                playerLoadouts.Add(Tank.MakePlayer(100, [weapon]));
                foreach (var armor in armors)
                {
                    playerLoadouts.Add(Tank.MakePlayer(100, [weapon, armor]));
                    foreach (var ring in rings)
                    {
                        playerLoadouts.Add(Tank.MakePlayer(100, [weapon, ring]));
                        playerLoadouts.Add(Tank.MakePlayer(100, [weapon, armor, ring]));
                        foreach (var ring2 in rings)
                        {
                            if (ring.Name != ring2.Name)
                            {
                                playerLoadouts.Add(Tank.MakePlayer(100, [weapon, ring, ring2]));
                                playerLoadouts.Add(Tank.MakePlayer(100, [weapon, armor, ring, ring2]));
                            }
                        }
                    }
                }
            }
            playerLoadouts.Sort((a, b) => a.TotalCost.CompareTo(b.TotalCost));
            LoadoutCache = playerLoadouts;
            return playerLoadouts;
        }
        public override ValueTask<string> Solve_1() => new($"{GetPossiblePlayerLoadoutsOrderedByCost().First(player => Battle(player)).TotalCost}");
        public override ValueTask<string> Solve_2() => new($"{Enumerable.Reverse(GetPossiblePlayerLoadoutsOrderedByCost()).First(player => !Battle(player)).TotalCost}");
    }
}
