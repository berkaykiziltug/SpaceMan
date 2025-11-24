using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
   [SerializeField] private AudioClip fuelPickupAudioClip;
   [SerializeField] private AudioClip coinPickupAudioClip;
   [FormerlySerializedAs("pickupMMFPlayer")] [SerializeField] private MMF_Player coinPickupMMFPlayer;
   [SerializeField] private MMF_Player fuelPickupMMFPlayer;
   private void Start()
   {
      Player.Instance.OnFuelPickup += Player_OnFuelPickup;
      Player.Instance.OnCoinPickup += Player_OnCoinPickup;
   }

   private void Player_OnCoinPickup(object sender, EventArgs e)
   {
      coinPickupMMFPlayer.PlayFeedbacks();
      // AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position);
   }

   private void Player_OnFuelPickup(object sender, EventArgs e)
   {
      fuelPickupMMFPlayer.PlayFeedbacks();
      // AudioSource.PlayClipAtPoint(fuelPickupAudioClip, Camera.main.transform.position);
   }
}
