using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI titleTMP;
  [SerializeField] private TextMeshProUGUI statsTMP;
  [SerializeField] private TextMeshProUGUI nextButtonTMP;
  [SerializeField] private Button nextButton;
  [SerializeField] private MMF_Player screenShakeFeedback;
  private Action nextButtonClickAction;

  private void Awake()
  {
    nextButton.onClick.AddListener(() =>
    {
      nextButtonClickAction();
    });
  }

  private void Start()
  {
    Player.Instance.OnLanded += InstanceOnLanded;
    Hide();
  }

  private void InstanceOnLanded(object sender, Player.OnLandedEventArgs e)
  {
    if (e.landingType == Player.LandingType.Success)
    {
      titleTMP.text = "SUCCESSFUL LANDING!";
      nextButtonTMP.text = "CONTINUE";
      nextButtonClickAction = GameManager.Instance.GoToNextLevel;
    }
    else
    {
      screenShakeFeedback.PlayFeedbacks();
      titleTMP.text = "<color=#ff0000>CRASH!</color>";
      nextButtonTMP.text = "RETRY";
      nextButtonClickAction = GameManager.Instance.RetryLevel;
    }

    statsTMP.text =
      Mathf.Round(e.landingSpeed * 2f) + "\n" +
      Mathf.Round(e.dotVector * 100f )+ "\n" +
      "x" + e.scoreMultiplier + "\n" +
      e.onLandedScore;
      Show();
  }

  private void Show()
  {
    gameObject.SetActive(true);
  }

  private void Hide()
  {
    gameObject.SetActive(false);
  }
}
