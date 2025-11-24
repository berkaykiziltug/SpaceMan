using UnityEngine;

public class GameInput : MonoBehaviour
{
   public static GameInput Instance {get; private set;}
   private InputActions inputActions;
   private void Awake()
   {
       if (Instance != null && Instance != this)
       {
           Destroy(gameObject);
       }
       else
       {
           Instance = this;
       }
       inputActions = new InputActions();
       inputActions.Enable();
   }

   public bool IsUpActionPressed()
   {
      return inputActions.Player.PlayerUp.IsPressed();
   }
   public bool IsLeftActionPressed()
   {
       return inputActions.Player.PlayerLeft.IsPressed();
   }
   
   public bool IsRightActionPressed()
   {
       return inputActions.Player.PlayerRight.IsPressed();
   }

   private void OnDestroy()
   {
       inputActions.Disable();
   }
}
