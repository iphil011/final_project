using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.BT;
using AlanZucconi.Bots;

// Does nothing
[CreateAssetMenu(fileName = "BotAI_Idle", menuName = "Bots/BotAI_Idle")]
public class BotAI_Idle : BotAI
{
    public override Node CreateBehaviourTree(Bot bot)
    {
        return Action.Nothing;
    }
}