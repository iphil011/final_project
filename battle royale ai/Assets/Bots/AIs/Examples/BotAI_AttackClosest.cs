using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.BT;
using AlanZucconi.Bots;

// Approaches and attack the closest
[CreateAssetMenu(fileName = "BotAI_AttackClosest", menuName = "Bots/BotAI_AttackClosest")]
public class BotAI_AttackClosest : BotAI
{
    public override Node CreateBehaviourTree(Bot bot)
    {
        return new Filter
        (
            bot.EnemiesInSight,
            new Selector
            (
                // Approaches if far enough
                new Filter
                (
                    () => bot.GetRange(bot.ClosestEnemy) > 0.5f,
                    new Action( () => bot.Approach(bot.ClosestEnemy) )
                ),
                // Shoot if far enough
                new Action( () => bot.Attack(bot.ClosestEnemy) )
            )
        );
    }
}