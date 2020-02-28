using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UserInterfaceManager : SuperClass
{
    public Text score;
    public Text widthValueText;
    public Text heightValueText;
    public Text colorCountText;
    public Slider widthSlider;
    public Slider heightSlider;
    public Slider colorCountSlider;
    public GameObject preparationScreen;
    public GameObject colorSelectionParent;
    public GameObject gameOverScreen;
    public List<Color> availableColors;
   

    private GridManager GridManagerObject;
    private int colorCount;
    private int blownHexagons;
    private int bombCount;

    public static UserInterfaceManager instance;



    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        bombCount = ZERO;
        GridManagerObject = GridManager.instance;
        blownHexagons = ZERO;
        colorCount = 7;
        InitializeUI();
    }

    public void Score(int x)
    {
        blownHexagons += x;
        score.text = (SCORE_CONSTANT * blownHexagons).ToString(); // Scoring is 5 times of blown hexagons
        if (Int32.Parse(score.text) > BOMB_SCORE_THRESHOLD * bombCount + BOMB_SCORE_THRESHOLD) // Need 1000 point for every bomb
        {
            ++bombCount;
            GridManagerObject.SetBombProduction();
        }
    }

    public void GameEnd()
    {
        gameOverScreen.SetActive(true);
    }

    public void BackButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void WidthSliderChange()
    {
        /* This will allow only odd numbers to protect symmetrical visual */
        widthValueText.text = ((widthSlider.value - MINIMUM_GRID_WIDTH) * DOUBLE + MINIMUM_GRID_WIDTH).ToString();
    }

    public void HeightSliderChange()
    {
        heightValueText.text = heightSlider.value.ToString();
    }

    public void ColorCountSliderChange()
    {
        int childCount = colorSelectionParent.transform.childCount;
        int newCount = (int)colorCountSlider.value;
        colorCountText.text = newCount.ToString();


        if (newCount > colorCount)
        {
            for (int i = 0; i < newCount; ++i)
            {
                if (!colorSelectionParent.transform.GetChild(i).gameObject.activeSelf)
                {
                    colorSelectionParent.transform.GetChild(i).gameObject.SetActive(true);                 
                }
            }
        }

        else if (newCount < colorCount)
        {
            for (int i = colorCount-1; i >= newCount; --i)
            {
                colorSelectionParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        colorCount = newCount;
    }



    /* Sends options to required objects and starts game */
    public void StartGameButton()
    {
        preparationScreen.SetActive(false);
        GridManagerObject.SetGridHeight((int)heightSlider.value);
        GridManagerObject.SetGridWidth((int)(widthSlider.value - MINIMUM_GRID_WIDTH) * DOUBLE + MINIMUM_GRID_WIDTH);
        GridManagerObject.Colors = GetColorList();
        GridManagerObject.SetColorList(GridManagerObject.Colors.Take((int)colorCountSlider.value).ToList());
        GridManagerObject.InitializeGrid();
    }

   
    public List<Color> GetColorList()
    {

        List<Color> colors = new List<Color>();

        colors.Add(Color.blue);
        colors.Add(Color.red);
        colors.Add(Color.yellow);
        colors.Add(Color.green);
        colors.Add(Color.cyan);
        colors.Add(Color.magenta);
        colors.Add(Color.white);

        return colors;
    }

    /* Sets default values to UI elements */
    private void InitializeUI()
    {
        Default();
        for (int i = 0; i < colorSelectionParent.transform.childCount - colorCount; ++i)
        {
            colorSelectionParent.transform.GetChild(colorSelectionParent.transform.childCount - i - 1).gameObject.SetActive(false);
        }
    }
    /* Assigns all options to default values */
    public void Default()
    {
        heightSlider.value = DEFAULT_GRID_HEIGHT;
        widthSlider.value = DEFAULT_GRID_WIDTH;
        colorCountSlider.value = DEFAULT_COLOR_COUNT;
        colorCount = DEFAULT_COLOR_COUNT;
        widthValueText.text = ((widthSlider.value - MINIMUM_GRID_WIDTH) * DOUBLE + MINIMUM_GRID_WIDTH).ToString();
        heightValueText.text = heightSlider.value.ToString();
        score.text = blownHexagons.ToString();
    }
}
