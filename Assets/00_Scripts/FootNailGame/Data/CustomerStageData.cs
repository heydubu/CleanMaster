using System.Collections.Generic;
using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "CustomerStageData", menuName = "FootNailGame/Customer")]
    public class CustomerStageData : ScriptableObject
    {
        public string customerStageId;
        public string customerName;
        public Sprite customerPortrait;
        public Sprite footPreview;
        public Sprite beforeNailSprite;
        public Sprite afterNailSprite;
        public string requestText;
        public List<TreatmentStepData> treatmentSteps = new List<TreatmentStepData>();
        public List<NailDesignData> availableNailDesigns = new List<NailDesignData>();
        public string requestedNailShapeId;
        public NailDesignData requestedNailDesign;
        public int clearRewardCoins;
        public int clearRewardSalonExp;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(customerStageId))
                Debug.LogWarning("[CustomerStageData] " + name + " is missing customerStageId.", this);

            if (treatmentSteps.Count == 0)
                Debug.LogWarning("[CustomerStageData] " + name + " has no treatmentSteps.", this);

            var seenTypes = new HashSet<TreatmentType>();
            foreach (TreatmentStepData step in treatmentSteps)
            {
                if (step == null)
                    continue;

                if (!seenTypes.Add(step.treatmentType))
                    Debug.LogWarning("[CustomerStageData] " + name + " has a duplicate TreatmentType in treatmentSteps: " + step.treatmentType + ".", this);
            }

            if (availableNailDesigns.Count == 0)
                Debug.LogWarning("[CustomerStageData] " + name + " has no availableNailDesigns.", this);
        }
    }
}
