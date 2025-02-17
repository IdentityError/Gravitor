﻿using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    private Color32 SELECTED_COLOR = new Color32(0, 225, 255, 225);
    private Color32 DESELECTED_COLOR = new Color32(225, 225, 225, 255);

    [SerializeField] private Text bronzeScore;
    [SerializeField] private Text silverScore;
    [SerializeField] private Text goldScore;
    [SerializeField] private Text bronzeGP;
    [SerializeField] private Text silverGP;
    [SerializeField] private Text goldGP;
    [SerializeField] private Text info;
    [SerializeField] private Text highScore;
    [SerializeField] private Image tutorialButton = null;

    [SerializeField] private GameObject levelInfoPanel;

    private Level selectedLevel;
    private PlayerData playerData;

    void Start()
    {
        AudioManager.GetInstance().currentMusic = AudioManager.GetInstance().PlaySound(AudioManager.MENU_SONG);
        playerData = SaveManager.GetInstance().LoadPersistentData(SaveManager.PLAYER_DATA).GetData<PlayerData>();
        tutorialButton.color = playerData.showTutorial ? SELECTED_COLOR : DESELECTED_COLOR;
    }

    public void LoadCurrentSelectedLevel()
    {
        LevelLoader.GetInstance().SetCurrentLevel(selectedLevel);

        SceneLoader loader = GetComponent<SceneLoader>();
        AudioManager.GetInstance().SmoothOutSound(AudioManager.GetInstance().currentMusic, 0.05f, 1f);
        loader.LoadSceneAsynchronously(SceneLoader.MAP_NAME);
    }

    public void OpenLevelInfo(Level level)
    {
        levelInfoPanel.SetActive(true);
        bronzeScore.text = " > " + level.bronzeScore.ToString("0");
        silverScore.text = " > " + level.silverScore.ToString("0");
        goldScore.text = " > " + level.goldScore.ToString("0");
        bronzeGP.text = level.bronzeGP.ToString("0");
        silverGP.text = level.silverGP.ToString("0");
        goldGP.text = level.goldGP.ToString("0");
        highScore.text = "Highscore: " + SaveManager.GetInstance().LoadPersistentData(SaveManager.LEVELSDATA_PATH).GetData<LevelsData>().GetLevelHighScore(level.id).ToString("0");
        info.text = level.levelInfo;
        selectedLevel = level;
    }

    public void CloseLevelInfo()
    {
        selectedLevel = null;
        levelInfoPanel.SetActive(false);
    }

    public void ToggleTutorial()
    {
        playerData.showTutorial = !playerData.showTutorial;
        tutorialButton.color = playerData.showTutorial ? SELECTED_COLOR : DESELECTED_COLOR;
        SaveManager.GetInstance().SavePersistentData(playerData, SaveManager.PLAYER_DATA);
    }
}
