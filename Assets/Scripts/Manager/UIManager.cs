using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    [SerializeField]
    private MenuPanel menuPanel;
    public MenuPanel MenuPanel => menuPanel;

    [SerializeField]
    private SettingPanel settingPanel;
    public SettingPanel SettingPanel => settingPanel;

    [SerializeField]
    private GamePanel gamePanel;
    public GamePanel GamePanel => gamePanel;

    [SerializeField]
    private PausePanel pausePanel;
    public PausePanel PausePanel => pausePanel;

    [SerializeField]
    private VictoryPanel victoryPanel;
    public VictoryPanel VictoryPanel => victoryPanel;

    [SerializeField]
    private LosePanel losePanel;
    public LosePanel LosePanel => losePanel;

    [SerializeField]
    private LoadingPanel loadingPanel;
    public LoadingPanel LoadingPanel => loadingPanel;

    [SerializeField]
    private ShowOnMobile showOnMobiePanel;
    public ShowOnMobile ShowOnMobilePanel => showOnMobiePanel;

    private void Start()
    {
        ActiveMenuPanel(true);
        ActiveSettingPanel(false);
        ActiveGamePanel(false);
        ActivePausePanel(false);
        ActiveVictoryPanel(false);
        ActiveLosePanel(false);
        ActiveLoadingPanel(false);
        ActiveShowOnMobilePanel(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.HasInstance && GameManager.Instance.IsPlaying == true)
            {
                GameManager.Instance.PauseGame();
                ActivePausePanel(true);
                ActiveShowOnMobilePanel(false);
                AudioManager.Instance.PlaySE(AUDIO.SE_013_CONFIRM);
            }
        }
    }

    public void ActiveMenuPanel(bool active)
    {
        menuPanel.gameObject.SetActive(active);
    }

    public void ActiveSettingPanel(bool active)
    {
        settingPanel.gameObject.SetActive(active);
    }

    public void ActiveGamePanel(bool active)
    {
        gamePanel.gameObject.SetActive(active);
    }

    public void ActivePausePanel(bool active)
    {
        pausePanel.gameObject.SetActive(active);
    }

    public void ActiveVictoryPanel(bool active)
    {
        victoryPanel.gameObject.SetActive(active);
    }

    public void ActiveLosePanel(bool active)
    {
        losePanel.gameObject.SetActive(active);
    }

    public void ActiveLoadingPanel(bool active)
    {
        loadingPanel.gameObject.SetActive(active);
    }

    public void ActiveShowOnMobilePanel(bool active)
    {
        showOnMobiePanel.gameObject.SetActive(active);
    }
}