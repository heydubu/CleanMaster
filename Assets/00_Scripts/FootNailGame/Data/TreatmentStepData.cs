using System.Collections.Generic;
using UnityEngine;

namespace FootNailGame
{
    [System.Serializable]
    public class TreatmentStepData
    {
        public string stepId;
        public TreatmentType treatmentType;
        public ToolData requiredTool;
        public string instructionText;
        public Sprite startSprite;
        public Sprite completedSprite;
        public float baseRequiredProgress = 1f;
        public float baseTimeLimit;
        public int baseMaxMistakes = 10;
        public int baseTargetCount = 1;
        public float baseTargetScale = 1f;
        public float accuracyWeight = 1f;
        public bool isOptional;
        public List<DiagnosisOptionData> diagnosisOptions = new List<DiagnosisOptionData>();
        public string correctDiagnosisId;
    }
}
