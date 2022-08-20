using System.Collections;
using System.Collections.Generic;
using Creatures.Player.Behaviour;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

namespace Creatures.Mobs
{
    public class NavMeshBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface surface;
        [SerializeField] private PlayerMovement player;
        [SerializeField] private float updateRate = 0.1f;
        [SerializeField] private float movementThreshold = 3f;
        [SerializeField] private Vector3 navMeshSize = new Vector3(20, 20, 20);

        private Vector3 _worldAnchor;
        private NavMeshData _navMeshData;
        private List<NavMeshBuildSource> _sources = new List<NavMeshBuildSource>();
        
        // Start is called before the first frame update
        private void Start()
        {
            _navMeshData = new NavMeshData();
            NavMesh.AddNavMeshData(_navMeshData);
            BuildNavMesh(false);
            StartCoroutine(CheckPlayerMovement());

        }

        private IEnumerator CheckPlayerMovement()
        {
            WaitForSeconds Wait = new WaitForSeconds(updateRate);
            while (true)
            {
                if (Vector3.Distance(_worldAnchor, player.transform.position) > movementThreshold)
                {
                    BuildNavMesh(true);
                    _worldAnchor = player.transform.position;
                }

                yield return null;
            }
        }

        private void BuildNavMesh(bool isAsync)
        {
            Bounds navMeshBounds = new Bounds(player.transform.position, navMeshSize);
            List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();

            List<NavMeshModifier> modifiers;
            if (surface.collectObjects == CollectObjects.Children)
                modifiers = new List<NavMeshModifier>(surface.GetComponentsInChildren<NavMeshModifier>());
            else
                modifiers = NavMeshModifier.activeModifiers;

            foreach (var mod in modifiers)
            {
                if ((surface.layerMask & (1 << mod.gameObject.layer)) == 1 && mod.AffectsAgentType(surface.agentTypeID))
                {
                    markups.Add(new NavMeshBuildMarkup
                    {
                        root = mod.transform,
                        overrideArea =mod.overrideArea,
                        area = mod.area,
                        ignoreFromBuild = mod.ignoreFromBuild
                    });
                }
            }

            if (surface.collectObjects == CollectObjects.Children)
                NavMeshBuilder.CollectSources(surface.transform, surface.layerMask, surface.useGeometry,
                    surface.defaultArea, markups, _sources);
            else
                NavMeshBuilder.CollectSources(navMeshBounds, surface.layerMask, surface.useGeometry,
                    surface.defaultArea, markups, _sources);

            _sources.RemoveAll(source =>
                source.component != null && source.component.gameObject.GetComponent<NavMeshAgent>() != null);
        }
    }
}
