using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    [Spell("Wave", "Throws a magic wave which deals splash attack to enemies on its way.", 
        new MagicElement[] {MagicElement.Light, MagicElement.Magma, MagicElement.Crystal})]
    [ElementRestrictions(MagicElement.Light | MagicElement.Magma | MagicElement.Crystal)]
    public class WaveSpell : Spell
    {
        private const int prefabID = 3;

        [IncreasableProperty(2, 1.1, MagicElement.Light)]
        public double WaveDamage { get; set; } = 12;
        [IncreasableProperty(1, MagicElement.Magma)]
        public double WaveLength { get; set; } = 10;
        [IncreasableProperty(1.2, MagicElement.Crystal, IncreasablePropertyAttribute.IncreaseMode.Multiplication)]
        public double WaveStartSize { get; set; } = 1;

        public override void Cast(GameObject player, PlayerMagic magic)
        {
            Caster = player;
            var variableForPrefab = magic.GetSpellPrefabByID(prefabID);
            var spellObject = UnityEngine.Object.Instantiate(variableForPrefab);
            spellObject.GetComponent<WaveScript>().Configuration = this;
            spellObject.transform.position = player.transform.position;
            Ray mouseCastPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane worldPlane = new Plane(Vector3.up, player.transform.position);
            worldPlane.Raycast(mouseCastPoint, out float enter);
            Vector3 castPos = mouseCastPoint.GetPoint(enter);
            spellObject.transform.forward = castPos;
        }
    }
}
