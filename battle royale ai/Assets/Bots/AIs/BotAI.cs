using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.Data;
using AlanZucconi.AI.BT;

namespace AlanZucconi.Bots
{
    // It constructs a behaviour tree (Factory)
    // but does not hold any data
    // We can reuse this ScriptableObject for multiple bots
    public abstract class BotAI : ScriptableObject
    {
        [Header("Student Data")]
        public string StudentLogin = "yourlogin";
        public string StudentName = "FirstName LastName";
        public string StudentEmail = "youremail@gold.ac.uk";

        [Header("AI Data")]
        public string AIName = "AI Name";
        [TextArea(1, 5)]
        public string AIDescription = "The description of your AI";

        //[HideInInspector]
        //public Bot Bot;

        //[HideInInspector]
        //[ScatterPlot]
        //public List<Vector2Int> Data = new List<Vector2Int>();
        //[ScatterPlot(LabelX = "ticks", LabelY = "points")]
        //public PlotData PlotData = new PlotData();

        //[ScatterPlot(LabelX = "Bots alive", LabelY = "Enemies alive")]
        //public PlotData PlotData = new PlotData();

        [HideInInspector]
        public BehaviourTree Tree;

        /*
        public void Initialise()
        {
            Tree = new BehaviourTree(CreateBT());
        }
        */
        /*
        public void Update()
        {
            Tree.Update();
        }
        */

        public virtual Node CreateBehaviourTree(Bot bot)
        {
            return null;
        }

        // This method is called when a new simulation starts
        // and can be used to initialise variabiles that are shared 
        // across bots of the same team (= ScriptableObject/BotAI)
        public virtual void Initialise ()
        {

        }
    }
}