using System;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
   [SerializeField] private Button resumeButton;
   [SerializeField] private Button mainMenuButton;

   private void Awake()
   {
      resumeButton.onClick.AddListener(() =>
      {
         GameManager.Instance.UnpauseGame();
      });
      mainMenuButton.onClick.AddListener(() =>
      {
         SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
      });
   }

   private void Start()
   {
      GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
      GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnPaused;
      Hide();
   }

   private void GameManager_OnGameUnPaused(object sender, EventArgs e)
   {
      Hide();
   }

   private void GameManager_OnGamePaused(object sender, EventArgs e)
   {
      Show();
   }

   public void Show()
   {
      gameObject.SetActive(true);
   }

   public void Hide()
   {
      gameObject.SetActive(false);
   }
}
