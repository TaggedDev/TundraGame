using System;
using UnityEngine;

namespace Bestiary
{
    /// <summary>
    /// Object to parse the JSON with mob values
    /// </summary>
    public class BestiaryParser
    {
        private TextAsset _JSONfile;
        private BestiaryMob[] _mobs;

        public BestiaryParser(TextAsset jsonFile)
        {
            _JSONfile = jsonFile;
        }
        
        /// <summary>
        /// Parses the JSON file passed in constructor as BestiaryMob object
        /// </summary>
        /// <returns>The array of mobs represented as BestiaryMob[]</returns>
        public BestiaryMob[] GetMobList()
        {
            BestiaryMobs jsonMobs = JsonUtility.FromJson<BestiaryMobs>(_JSONfile.text);
            _mobs = jsonMobs.mobs;

            if (_mobs == null)
                throw new Exception("No mobs found. Make sure the JSON file is uploaded or correct");
            
            /*foreach (var mob in jsonMobs.mobs)
             {Debug.Log($"Found mob {mob.mobName}, {mob.mobDescription}, {mob.mobAvatarPath}");}*/
            return _mobs;
        }
    }
}