using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUI.HeadUpDisplay
{
    /// <summary>
    /// A class to control recipe tile.
    /// </summary>
    public class CraftTileUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private GameObject recipePartIconPrefab;
        [SerializeField]
        private GameObject recipeTileIcon;
        [SerializeField]
        private GameObject recipeName;
        [SerializeField]
        private Image progressImage;
        [SerializeField]
        private float craftTime;

        private GameObject[] _recipeParts;
        private bool _isCrafting;
        private float _progress;
        private RecipeConfiguration _recipe;
        private PlayerInventory _playerInventory;
        private bool _isAvailable;

        public event EventHandler RecipeCrafted;

        public void SetRecipe(RecipeConfiguration recipe, PlayerInventory inventory, bool isAvailable)
        {
            _recipe = recipe;
            _playerInventory = inventory;
            GetComponent<Button>().interactable = _isAvailable = isAvailable;
            recipeTileIcon.GetComponent<Image>().sprite = recipe.Result.Icon;
            recipeName.GetComponent<Text>().text = recipe.Result.Title;
            if (_recipeParts != null)
            {
                foreach (var part in _recipeParts)
                {
                    Destroy(part);
                }
            }
            _recipeParts = new GameObject[recipe.RequiredItems.Count];
            Vector3 offset = Vector3.zero;
            offset.x += 140;
            foreach (var item in recipe.RequiredItems)
            {
                var part = Instantiate(recipePartIconPrefab, transform);
                part.transform.localPosition = offset;
                part.transform.Find("ItemIcon").gameObject.GetComponent<Image>().sprite = item.Item.Icon;
                part.transform.Find("AmountIndicator").gameObject.GetComponent<Text>().text = item.Amount.ToString();
                part.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                offset.x += 125;
            }
        }

        private void Update()
        {
            if (_isCrafting)
            {
                if (Input.GetMouseButton(0))
                {
                    _progress += Time.deltaTime;
                    progressImage.fillAmount = _progress / craftTime;
                    if (_progress > craftTime)
                    {
                        _recipe.Craft(_playerInventory, out int slot);
                        if (slot != -1) _playerInventory.SelectedInventorySlot = slot;
                        RecipeCrafted?.Invoke(this, null);
                        CloseCraft();
                    }
                }
                else
                {
                    CloseCraft();
                }
            }
        }

        private void CloseCraft()
        {
            _isCrafting = false;
            _progress = 0;
            progressImage.fillAmount = 0;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isCrafting && _isAvailable) _isCrafting = true;
        }
    }
}