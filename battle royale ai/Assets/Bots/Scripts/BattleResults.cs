using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using AlanZucconi.Data;

namespace AlanZucconi.Bots
{
    /*
    [System.Serializable]
    public class BotAITuple : Tuple<BotAI, BotAI>
    {
        public BotAITuple(BotAI a, BotAI b) : base(a, b) { }
    }
    */

    [System.Serializable]
    public struct BotAITuple
    {
        public BotAI First;
        public BotAI Second;

        public BotAITuple (BotAI first, BotAI second)
        {
            First = first;
            Second = second;
        }
    }

    // Vector2 is not serializable
    [System.Serializable]
    public class Vector2List : List<Vector2>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<float> x = new List<float>();

        [SerializeField]
        private List<float> y = new List<float>();

        public void OnBeforeSerialize()
        {
            x.Clear();
            y.Clear();
            foreach (Vector2 v in this)
            {
                x.Add(v.x);
                y.Add(v.y);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < x.Count; i++)
                Add(new Vector2(x[i], y[i]));
        }
    }

    // Dictionary[AI_A, AI_B] = (bot A alive, bot B alive)
    [System.Serializable]
    public class SerializableBattleDictionary : SerializableDictionary<BotAITuple, Vector2List> { }

    // Used to store the results of a Battle RoyAIe tournament
    [CreateAssetMenu(fileName = "BotAI_yourlogin_results", menuName = "Bots/Battle Results", order = 0)]
    public class BattleResults : ScriptableObject
    {
        // Dictionary[AI_A, AI_B] = (bot A alive, bot B alive)    
        [SerializeField]
        public SerializableBattleDictionary BattleScores = new SerializableBattleDictionary();

        // List of AIs used
        public List<BotAI> AIs = new List<BotAI>();

        public void AddBattleResult(BotAI aiA, BotAI aiB, float botsAAlive, float botsBAlive)
        {
            //Tuple<BotAI, BotAI> key = new Tuple<BotAI, BotAI>(aiA, aiB);
            BotAITuple key = new BotAITuple(aiA, aiB);
            //List<Vector2> scores;
            Vector2List scores;
            if (!BattleScores.TryGetValue(key, out scores))
            {
                // Creates a new list
                //scores = new List<Vector2>();
                scores = new Vector2List();
                BattleScores.Add(key, scores);

                // Updates the list of AIs
                if (!AIs.Contains(aiA))
                    AIs.Add(aiA);
                if (!AIs.Contains(aiB))
                    AIs.Add(aiB);
            }

            // Adds the score to the list
            Vector2 score = new Vector2(botsAAlive, botsBAlive);
            scores.Add(score);

            Dirty();
        }

        public void Dirty()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        //[ScatterPlot(LabelX = "Bots alive", LabelY = "Enemies alive")]
        //public PlotData PlotData = new PlotData();

        [GridPlot(Trace = false)]
        public GridData AverageScores = null;

        [Button(Editor = true)]
        public void UpdateAverageScores()
        {
            // Initialises the AI
            AverageScores = new GridData(AIs.Count, AIs.Count);
            AverageScores.LabelsR = AverageScores.LabelsC = AIs.Select(ai => ai.AIName).ToArray();



            //foreach (BotAI aiA in AIs)
            for (int r = 0; r < AIs.Count; r++)
            {
                BotAI aiA = AIs[r];
                for (int c = 0; c < AIs.Count; c++)
                {
                    BotAI aiB = AIs[c];

                    //Tuple<BotAI, BotAI> key = new Tuple<BotAI, BotAI>(aiA, aiB);
                    BotAITuple key = new BotAITuple(aiA, aiB);
                    //List<Vector2> scores;
                    Vector2List scores;
                    if (BattleScores.TryGetValue(key, out scores))
                    {
                        AverageScores[r, c] = scores
                            .Select(score => score.x - score.y)
                            .Average();
                    }
                }
            }

            // Scores
            Dirty();
        }

        [Button(Editor = true)]
        public void ClearScores()
        {
            AIs.Clear();
            BattleScores.Clear();
            AverageScores = null;
            Dirty();
        }
    }
}