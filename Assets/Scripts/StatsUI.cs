using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private Image fuelBar;
    


    private void Update()
    {
        UpdateStatsTextMesh();
        UpdateFuelBar();
    }
    
    private void UpdateStatsTextMesh()
    {
        speedUpArrowGameObject.SetActive(Player.Instance.GetSpeedY() >= 0);
        speedDownArrowGameObject.SetActive(Player.Instance.GetSpeedY() < 0);
        speedLeftArrowGameObject.SetActive(Player.Instance.GetSpeedX() < 0);
        speedRightArrowGameObject.SetActive(Player.Instance.GetSpeedX() >= 0);
        statsTextMesh.text = GameManager.Instance.GetLevel() + "\n"+
                             GameManager.Instance.GetScore() + "\n" +
                             Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
                             Mathf.Abs(Mathf.Round(Player.Instance.GetSpeedX() * 10f)) + "\n" +
                             Mathf.Abs(Mathf.Round(Player.Instance.GetSpeedY() * 10f)) + "\n";

    }

    private void UpdateFuelBar()
    {
        // fuelBar.fillAmount = Player.Instance.GetFuelAmountNormalized();

        fuelBar.DOFillAmount(Player.Instance.GetFuelAmountNormalized(), Player.Instance.GetFuelAmountNormalized() > .5 ? .4f : .3f);
    }
    
    
    
    
}
