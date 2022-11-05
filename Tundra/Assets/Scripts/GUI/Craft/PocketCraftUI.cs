using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class PocketCraftUI : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private CraftHelper _craftHelper;
    private RecipeCofiguration[] _basicRecipes;
    private int _selectedRecipe;
    private GameObject _segmentHolder;
    private RecipeCofiguration _currentRecipe;
    private GameObject[] _recipeParts;
    private Transform _centerCircle;
    private Image[] _recipeImages;
    private Text[] _recipeTexts;

    [SerializeField]
    private GameObject recipeIndicatorPrefab;
    [SerializeField]
    private int recipeSpacing;

    private int SelectedRecipe 
    {
        get => _selectedRecipe;
        set
        {
            if (_selectedRecipe != value && _basicRecipes.Length == 4 &&
                0 <= value && value <= 4 && _basicRecipes[value].CheckIfAvailable(null, _playerInventory))
            {
                _selectedRecipe = value;
                SelectedRecipeIndexChanged();
            }
        }
    }

    private void SelectedRecipeIndexChanged()
    {
        float rot = SelectedRecipe * 90;
        _segmentHolder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rot));
        _currentRecipe = _basicRecipes[_selectedRecipe];
        int items = _currentRecipe.RequiredItems.Count;
        Rect indicatorRect = (recipeIndicatorPrefab.transform as RectTransform).rect;
        float indicHeights = items * indicatorRect.height;
        float spacings = recipeSpacing * (items - 1);
        float totalSpacing = (indicHeights + spacings) * 0.5f;
        if (_recipeParts != null)
            foreach (var part in _recipeParts)
            {
                Destroy(part);
            }
        _recipeParts = new GameObject[items];
        for (int i = 0; i < items; i++)
        {
            var rec = Instantiate(recipeIndicatorPrefab, _centerCircle);
            _recipeParts[i] = rec;
            rec.transform.localPosition = new Vector3(-indicatorRect.width / 2, (indicatorRect.height + recipeSpacing) * i - totalSpacing + indicatorRect.height / 2);
            var image = rec.GetComponent<Image>();
            image.sprite = _currentRecipe.RequiredItems[i].Item.Icon;
            rec.GetComponentInChildren<Text>().text = "x" + _currentRecipe.RequiredItems[i].Amount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _craftHelper = CraftHelper.Instance;
        _playerInventory = player.GetComponent<PlayerInventory>();
        _basicRecipes = _craftHelper.BasicRecipes.OrderBy(x => x.name).ToArray();
        _segmentHolder = transform.Find("ElementSelectionCircle").Find("SegmentHolder").gameObject;
        _centerCircle = transform.Find("ElementSelectionCircle").Find("CenterCircle");
        _recipeImages = new Image[4];
        _recipeTexts = new Text[4];
        for (int i = 0; i < 4; i++)
        {
            var obj = transform.Find("ElementSelectionCircle").Find($"ElementIcon{i+1}").gameObject;
            _recipeImages[i] = obj.GetComponent<Image>();
            _recipeTexts[i] = obj.GetComponentInChildren<Text>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) gameObject.SetActive(false);
        Vector3 mousePosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
        if (Mathf.Abs(mousePosition.x) < Mathf.Abs(mousePosition.y))
        {
            if (mousePosition.y > 0)
            {
                SelectedRecipe = 0;
            }
            else
            {
                SelectedRecipe = 2;
            }
        }
        else
        {
            if (mousePosition.x > 0)
            {
                SelectedRecipe = 3;
            }
            else
            {
                SelectedRecipe = 1;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            _currentRecipe.Craft(_playerInventory);
        }
        for (int i = 0; i < _basicRecipes.Length; i++)
        {
            Color c;
            if (i == SelectedRecipe) c = Color.white;
            else if (_basicRecipes[i].CheckIfAvailable(null, _playerInventory)) c = Color.gray;
            else c = new Color(0.25f, 0.25f, 0.25f);
            _recipeImages[i].color = _recipeTexts[i].color = c;
        }
    }
}
