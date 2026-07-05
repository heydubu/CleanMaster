namespace FootNailGame
{
    [System.Serializable]
    public class StageDifficultyData
    {
        public float timeLimit;
        public int maxMistakes = 10;
        public float targetScale = 1.2f;
        public float toolEfficiencyMultiplier = 1.2f;
        public int dirtCount = 1;
        public int damagedAreaCount;
        public int targetCount = 1;
        public int simultaneousTargetCount = 1;
        public int requiredAccuracyPercent = 60;
        public int customerRequestCount = 1;
        public bool useHiddenTargets;
        public bool useMovingTargets;
        public bool requireCorrectToolOrder;
        public bool requireExactNailShape;
        public bool requireExactNailDesign;
        public bool failOnTimeOver;
        public bool failOnMistakeLimit;
    }
}
