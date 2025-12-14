using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Butuh ini untuk timer 10 detik

public class PizzaMaker : MonoBehaviour
{
    [Header("Sprite Referensi")]
    public Sprite spriteDough;      // Adonan Bulat
    public Sprite spriteFlat;       // Adonan Gepeng
    public Sprite spriteSauce;      // + Saus
    public Sprite spriteCheese;     // + Keju
    public Sprite spritePepperoni;  // + Paperoni
    public Sprite spriteVeggie;     // + Sayur (Mentah Siap Bake)
    public Sprite spriteBaked;

    [Header("Komponen UI")]
    public Button btnSauce;
    public Button btnCheese;
    public Button btnPepperoni;
    public Button btnVeggie;
    public Button btnBake;
    public Button btnSell;
    public Text textStatus; // Opsional: Buat nampilin tulisan "Baking..."

    private SpriteRenderer rend;
    private int doughClicks = 0; // Berapa kali adonan diklik
    private bool isBaking = false;

    // Status Pizza Kita
    private enum State { Dough, Flat, Sauced, Cheesed, Pepperoni, Veggie, Baked }
    private State currentState;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        ResetPizza();
    }

    // 1. Fungsi Klik Adonan (Dipanggil saat object diklik mouse)
    void OnMouseDown()
    {
        if (currentState == State.Dough)
        {
            doughClicks++;
            // Efek visual dikit: Adonan mengecil pas diklik lalu balik lagi
            transform.localScale = Vector3.one * 0.9f;
            Invoke("ResetScale", 0.1f);

            if (doughClicks >= 3) // Misal harus diklik 3 kali baru gepeng
            {
                currentState = State.Flat;
                rend.sprite = spriteFlat;
                UpdateUI();
            }
        }
    }

    void ResetScale() { transform.localScale = Vector3.one; }

    // 2. Fungsi Tombol Topping
    public void AddSauce()
    {
        if (currentState == State.Flat)
        {
            currentState = State.Sauced;
            rend.sprite = spriteSauce;
            UpdateUI();
        }
    }

    public void AddCheese()
    {
        if (currentState == State.Sauced)
        {
            currentState = State.Cheesed;
            rend.sprite = spriteCheese;
            UpdateUI();
        }
    }

    public void AddPepperoni()
    {
        if (currentState == State.Cheesed)
        {
            currentState = State.Pepperoni;
            rend.sprite = spritePepperoni;
            UpdateUI();
        }
    }

    public void AddVeggie()
    {
        if (currentState == State.Pepperoni)
        {
            currentState = State.Veggie;
            rend.sprite = spriteVeggie;
            UpdateUI();
        }
    }

    // 3. Fungsi Bake (Masak)
    public void BakePizza()
    {
        if (currentState == State.Veggie && !isBaking)
        {
            StartCoroutine(ProsesMasak());
        }
    }

    IEnumerator ProsesMasak()
    {
        isBaking = true;
        btnBake.interactable = false;

        if (textStatus != null) textStatus.text = "Sedang Memanggang...";

        // Tunggu 10 detik
        yield return new WaitForSeconds(5f);

        // --- UPDATE DI SINI ---
        currentState = State.Baked;
        isBaking = false;

        // Ganti gambar jadi Pizza Matang
        rend.sprite = spriteBaked;

        // Pastikan warnanya putih terang (normal)
        rend.color = Color.white;
        // ----------------------

        if (textStatus != null) textStatus.text = "MATANG! Siap Jual";
        UpdateUI();
    }

    // 4. Fungsi Jual
    public void JualPizza()
    {
        if (currentState == State.Baked)
        {
            // Panggil GameManager untuk usir NPC dan catat penjualan
            GameManager.instance.TombolJualDitekan();

            // Ulangi proses bikin pizza dari awal
            ResetPizza();
        }
    }

    void ResetPizza()
    {
        currentState = State.Dough;
        rend.sprite = spriteDough;
        rend.color = Color.white; // Balikin warna jadi terang
        doughClicks = 0;

        if (textStatus != null) textStatus.text = "Ketuk Adonan!";
        UpdateUI();
    }

    // Mengatur Tombol mana yang boleh ditekan
    void UpdateUI()
    {
        // Matikan semua dulu
        btnSauce.interactable = false;
        btnCheese.interactable = false;
        btnPepperoni.interactable = false;
        btnVeggie.interactable = false;
        btnBake.interactable = false;
        btnSell.gameObject.SetActive(false); // Tombol jual hilang kalau belum matang

        // Nyalakan sesuai urutan
        if (currentState == State.Flat) btnSauce.interactable = true;
        else if (currentState == State.Sauced) btnCheese.interactable = true;
        else if (currentState == State.Cheesed) btnPepperoni.interactable = true;
        else if (currentState == State.Pepperoni) btnVeggie.interactable = true;
        else if (currentState == State.Veggie) btnBake.interactable = true;
        else if (currentState == State.Baked) btnSell.gameObject.SetActive(true);
    }
}