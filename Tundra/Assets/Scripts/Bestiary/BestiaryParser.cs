using UnityEngine;

namespace World
{
    public class BestiaryParser : MonoBehaviour
    {
        public TextAsset JSONFile;
        public BestiaryMob[] Mobs;

        private void Start()
        {
            BestiaryMobs jsonMobs = JsonUtility.FromJson<BestiaryMobs>(JSONFile.text);
            Mobs = jsonMobs.mobs;
            foreach (var mob in jsonMobs.mobs)
            {
                Debug.Log($"Found mob {mob.mobName}, {mob.mobDescription}, {mob.mobAvatarPath}");   
            }
        }
    }
}