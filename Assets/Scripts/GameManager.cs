using UnityEngine;
using UnityEngine.UI; // Biar bisa akses UI

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Supaya mudah dipanggil dari mana saja

    [Header("Info Toko")]
    public NPCWalk customerSaatIni; // Siapa yang lagi dilayani?

    void Awake()
    {
        // Setup Singleton sederhana
        if (instance == null) instance = this;
    }

    // Fungsi dipanggil saat NPC sampai di Trigger Toko
    public void LaporAdaCustomer(NPCWalk npc)
    {
        customerSaatIni = npc;
        Debug.Log("Ada pembeli datang: " + npc.name);
    }

    // Fungsi ini yang nanti dipasang di TOMBOL JUAL
    public void TombolJualDitekan()
    {
        // Cek apakah ada orang di kasir?
        if (customerSaatIni != null)
        {
            // Suruh NPC pergi
            customerSaatIni.PesananSelesai();

            // Kosongkan data kasir
            customerSaatIni = null;

            Debug.Log("Pesanan terjual! Cuan ++");
        }
        else
        {
            Debug.Log("Belum ada pembeli woi, sabar.");
        }
    }
}