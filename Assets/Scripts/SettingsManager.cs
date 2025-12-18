using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Siapa tau nanti butuh tombol 'Exit/Restart'

public class SettingsManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject panelSetting; // Masukkan PanelSetting yang tadi kita buat
    public Slider sliderVolume;     // Masukkan SliderVolume di sini

    void Start()
    {
        // Pastikan panel mati saat awal game
        if (panelSetting != null)
            panelSetting.SetActive(false);

        // Atur slider sesuai volume sekarang (Default 1 alias 100%)
        if (sliderVolume != null)
        {
            sliderVolume.value = AudioListener.volume;
            // Pasang fungsi slider lewat kode biar rapi
            sliderVolume.onValueChanged.AddListener(AturVolume);
        }
    }

    // Fungsi dipanggil saat tombol Gear ditekan
    public void BukaMenu()
    {
        panelSetting.SetActive(true);
        Time.timeScale = 0f; // PAUSE GAME (Waktu berhenti)
    }

    // Fungsi dipanggil saat tombol X / Resume ditekan
    public void TutupMenu()
    {
        panelSetting.SetActive(false);
        Time.timeScale = 1f; // RESUME GAME (Waktu jalan lagi)
    }

    // Fungsi untuk Slider
    public void AturVolume(float nilai)
    {
        // AudioListener adalah 'telinga' utama di game. 
        // Mengubah ini akan mengubah volume SFX & Musik sekaligus.
        AudioListener.volume = nilai;
    }

    // Opsional: Tombol Keluar Game
    public void KeluarGame()
    {
        Application.Quit();
        Debug.Log("Keluar Game!");
    }
}