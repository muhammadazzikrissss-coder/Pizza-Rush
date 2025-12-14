using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada untuk pindah scene!

public class MainMenu : MonoBehaviour
{
    // Fungsi untuk tombol Play
    public void PlayGame()
    {
        // Ganti "GameScene" dengan nama file scene gameplay kamu yang PERSIS
        // Atau bisa pakai nomor index: SceneManager.LoadScene(1);
        SceneManager.LoadScene("MainScene");
    }

    // Fungsi untuk tombol Quit
    public void QuitGame()
    {
        Debug.Log("Keluar dari game..."); // Cuma muncul di editor
        Application.Quit();
    }
}