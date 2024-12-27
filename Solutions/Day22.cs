using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day22: BaseDayWithInput
    {
        readonly int startHP;
        readonly int startMana;
        readonly int startBossHP;
        readonly int bossDamage;
        public Day22()
        {
            startHP = 50;
            startMana = 500;
            startBossHP = int.Parse(_input[0].Split(": ")[1]);
            bossDamage = int.Parse(_input[1].Split(": ")[1]);
        }
        enum Turn
        {
            PlayerStart,
            Player,
            BossStart,
            Boss
        }
        struct Gamestate
        {
            public bool hardMode;
            public Turn turn;
            public int bossHP;
            public int playerHP;
            public int mana;
            public int shieldTimer;
            public int poisonTimer;
            public int rechargeTimer;
            public override readonly int GetHashCode()
                => HashCode.Combine(turn, bossHP, playerHP, mana, shieldTimer, poisonTimer, rechargeTimer, hardMode ? 1 : 0);
            //without override, it will use the default implementation and the bool tanks it to unusable
            //goes from 20ms to 2minutes!! another way is to use enum or int instead of bool
        }
        static private Gamestate TurnStart(Gamestate state)
        {
            if(state.turn == Turn.PlayerStart&&state.hardMode)
            {
                state.playerHP--;
            }
            if (state.shieldTimer > 0)
            {
                state.shieldTimer--;
            }
            if (state.poisonTimer > 0)
            {
                state.poisonTimer--;
                state.bossHP -= 3;
            }
            if (state.rechargeTimer > 0)
            {
                state.rechargeTimer--;
                state.mana += 101;
            }
            return state;
        }
        private Gamestate BossTurn(Gamestate state)
        {
            int damage = Math.Max(bossDamage - (state.shieldTimer > 0 ? 7 : 0), 1);
            state.playerHP -= damage;
            return state;
        }

        readonly Dictionary<Gamestate, int> DP_minManaToWin = [];
        int GetMinManaToWin(Gamestate state)
        {
            if (DP_minManaToWin.TryGetValue(state, out int value))
                return value;
            if (state.playerHP <= 0) return int.MaxValue;
            if (state.bossHP <= 0) return 0;
            switch (state.turn)
            {
                case Turn.PlayerStart:
                    state = TurnStart(state);
                    state.turn = Turn.Player;
                    return GetMinManaToWin(state);
                case Turn.BossStart:
                    state = TurnStart(state);
                    state.turn = Turn.Boss;
                    return GetMinManaToWin(state);
                case Turn.Boss:
                    state = BossTurn(state);
                    state.turn = Turn.PlayerStart;
                    return GetMinManaToWin(state);
                case Turn.Player:
                    if(state.mana< 53) return int.MaxValue;
                    List<(Gamestate nextState, int costToAdvance)> nextStates = [];
                    if (state.mana >= 53) // MAGIC MISSILE
                    {
                        Gamestate newState = state;
                        newState.mana -= 53;
                        newState.bossHP -= 4;
                        newState.turn = Turn.BossStart;
                        nextStates.Add((newState,53));
                    }
                    if (state.mana >= 73) // DRAIN
                    {
                        Gamestate newState = state;
                        newState.mana -= 73;
                        newState.bossHP -= 2;
                        newState.playerHP += 2;
                        newState.turn = Turn.BossStart;
                        nextStates.Add((newState, 73));
                    }
                    if (state.mana >= 113 && state.shieldTimer == 0) // SHIELD
                    {
                        Gamestate newState = state;
                        newState.mana -= 113;
                        newState.shieldTimer = 6;
                        newState.turn = Turn.BossStart;
                        nextStates.Add((newState, 113));
                    }
                    if (state.mana >= 173 && state.poisonTimer == 0) // POISON
                    {
                        Gamestate newState = state;
                        newState.mana -= 173;
                        newState.poisonTimer = 6;
                        newState.turn = Turn.BossStart;
                        nextStates.Add((newState, 173));
                    }
                    if (state.mana >= 229 && state.rechargeTimer == 0) // RECHARGE
                    {
                        Gamestate newState = state;
                        newState.mana -= 229;
                        newState.rechargeTimer = 5;
                        newState.turn = Turn.BossStart;
                        nextStates.Add((newState, 229));
                    }
                    int costToEnd = int.MaxValue;
                    foreach (var (nextState, costToAdvance) in nextStates)
                    {
                        var cost = GetMinManaToWin(nextState);
                        if (cost < int.MaxValue)
                            costToEnd = Math.Min(costToEnd, cost + costToAdvance);
                    }
                    DP_minManaToWin[state] = costToEnd;
                    return costToEnd;
                default:
                    throw new Exception("Invalid turn");
            }
        }
        public override ValueTask<string> Solve_1() => new($"{GetMinManaToWin(new() {
            hardMode = false,
            turn = Turn.PlayerStart,
            bossHP = startBossHP,
            playerHP = startHP,
            mana = startMana,
            shieldTimer = 0,
            poisonTimer = 0,
            rechargeTimer = 0
        })}");
        public override ValueTask<string> Solve_2() => new($"{GetMinManaToWin(new()
        {
            hardMode = true,
            turn = Turn.PlayerStart,
            bossHP = startBossHP,
            playerHP = startHP,
            mana = startMana,
            shieldTimer = 0,
            poisonTimer = 0,
            rechargeTimer = 0
        })}");
    }
}
