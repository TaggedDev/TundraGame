using Creatures.Player.Behaviour;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.HeadUpDisplay
{
    /// <summary>
    /// A class to control recipe tile.
    /// </summary>
    public class CraftTileUI : MonoBehaviour
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
        private float craftTime = 3f;

        private GameObject[] _recipeParts;
        private bool _isCrafting;
        private float _progress;
        private RecipeCofiguration _recipe;
        private PlayerInventory _playerInventory;

        public void SetRecipe(RecipeCofiguration recipe, PlayerInventory inventory)
        {
            _recipe = recipe;
            _playerInventory = inventory;
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
                    progressImage.fillAmount += _progress / craftTime;
                    if (_progress > craftTime)
                    {
                        _recipe.Craft(_playerInventory, out int slot);
                    }
                }
                else
                {
                    _isCrafting = false;
                    _progress = 0;
                }
            }
        }

        private void Click()
        {
            if (_recipe.CheckIfAvailable(_recipe.Workbench, _playerInventory)) _isCrafting = true;
        }
    }
}