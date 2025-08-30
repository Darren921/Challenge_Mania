using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
   
   public void LoadGame()
   {
      SceneManager.LoadScene("ChallengeSelection");
   }

   public void ToMainMenu()
   {
      SceneManager.LoadScene("TitleScreen");
   }
   
   public void QuitGame()
   {
      Application.Quit();
   }
}
