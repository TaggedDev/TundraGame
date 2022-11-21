using System.Collections.Generic;
using Creatures.Player.Crafts;
using System;
using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    /// <summary>
    /// Базовый класс для создания конфигураций предметов в игре.
    /// </summary>
    //[CreateAssetMenu(fileName = "New ItemConfiguration", menuName = "Items")]
    public abstract class BasicItemConfiguration : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] [Multiline(5)] private string description;
        [SerializeField] private GameObject itemInWorldPrefab;
        [SerializeField] private int maxStackVolume;
        [SerializeField] private float weight;
        [SerializeField] private RecipeConfiguration recipe;

        /// <summary>
        /// Иконка прмдета в инвентаре.
        /// </summary>
        public Sprite Icon { get => icon; protected set => icon=value; }
        
        /// <summary>
        /// Отображаемое название предмета.
        /// </summary>
        public string Title { get => title; protected set => title=value; }
        
        /// <summary>
        /// Описание предмета.
        /// </summary>
        public string Description { get => description; protected set => description=value; }
        
        /// <summary>
        /// Игровой объект, связанный с этим предметом (что будем бросать или отображать в мире).
        /// </summary>
        public GameObject ItemInWorldPrefab { get => itemInWorldPrefab; protected set => itemInWorldPrefab=value; }
        
        /// <summary>
        /// Максмальное количество предметов в стаке.
        /// </summary>
        public int MaxStackVolume { get => maxStackVolume; protected set => maxStackVolume=value; }
        
        /// <summary>
        /// Вес одного предмета в килограммах.
        /// </summary>
        public float Weight { get => weight; protected set => weight=value; }
        
        /// <summary>
        /// Рецпет для создания данного предмета.
        /// </summary>
        public RecipeConfiguration Recipe { get => recipe; protected set => recipe=value; }

        /// <summary>
        /// Drops an item into a world space.
        /// </summary>
        /// <param name="originPosition">Position to drop from.</param>
        /// <param name="throwForce">Force to apply to an item when drop.</param>
        /// <returns>The <see cref="GameObject"/> instance of a dropped item.</returns>
        public virtual GameObject Drop(Vector3 originPosition, Vector3 throwForce)
        {
            var obj = Instantiate(ItemInWorldPrefab, originPosition, Quaternion.identity);
            obj.TryGetComponent(out Rigidbody rigidbody);
            if (rigidbody != null)
                rigidbody.AddForce(throwForce);
            return obj;
        }

        /// <summary>
        /// Drop a few items into a world space.
        /// </summary>
        /// <param name="amount">Amount of items.</param>
        /// <param name="originPosition">Position to drop from.</param>
        /// <param name="throwForce">Force to apply.</param>
        /// <returns>List of <see cref="GameObject"/> with items instances.</returns>
        public virtual List<GameObject> MassDrop(int amount, Vector3 originPosition, Vector3 throwForce)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                result.Add(Drop(originPosition, throwForce));
            }
            return result;
        }
    }
}
