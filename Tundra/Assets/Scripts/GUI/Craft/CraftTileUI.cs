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

        private GameObject[] _recipeParts;


        public void SetRecipe(RecipeCofiguration recipe)
        {
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
            offset.x += 32;
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
    }
}