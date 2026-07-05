using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "NailDesignData", menuName = "FootNailGame/Nail Design")]
    public class NailDesignData : ScriptableObject
    {
        public string designId;
        public string displayName;
        public string countryCode;
        public string category;
        public Sprite thumbnail;
        public Sprite nailSprite;
        public int rarity;
        public int unlockPrice;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(designId))
                Debug.LogWarning("[NailDesignData] " + name + " is missing designId.", this);

            if (rarity < 0)
                Debug.LogWarning("[NailDesignData] " + name + " has a negative rarity.", this);

            if (unlockPrice < 0)
                Debug.LogWarning("[NailDesignData] " + name + " has a negative unlockPrice.", this);
        }
    }
}
