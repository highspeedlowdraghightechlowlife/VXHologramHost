//Vladislava Simakov
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ConversationResult
{

    [Serializable]
    public class Entity
    {
        public string category;
        public string text;
    }
    [Serializable]
    public class Prediction
    {
        public string topIntent;
        public Entity[] entities;

    }

    [Serializable]
    public class ResultData
    {
        public string query;
        public Prediction prediction;
    }

    public string kind;
    public ResultData result;
}

