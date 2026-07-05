namespace FootNailGame
{
    // Runtime copy of a TreatmentStepData, built by TreatmentFlowController.BuildRuntimeSteps.
    // Never write back into the source TreatmentStepData or its owning ScriptableObject.
    public class RuntimeTreatmentStepData
    {
        public TreatmentStepData source;
        public TreatmentType treatmentType;
        public ToolData requiredTool;
        public float requiredProgress;
        public float timeLimit;
        public int maxMistakes;
        public int targetCount;
        public int simultaneousTargetCount;
        public float targetScale;
        public float toolEfficiencyMultiplier;
        public int requiredAccuracyPercent;
        public bool useHiddenTargets;
        public bool useMovingTargets;
        public bool requireCorrectToolOrder;
    }
}
