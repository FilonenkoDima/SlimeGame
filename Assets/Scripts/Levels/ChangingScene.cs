using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangingScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Die()
    {
        SceneManager.LoadScene("Level1");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Level_Die(){
        SceneManager.LoadScene("Die_scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
