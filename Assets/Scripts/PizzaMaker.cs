using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PizzaMaker : MonoBehaviour
{
    [Header("Sprite Referensi")]
    public Sprite spriteDough;
    public Sprite spriteFlat;
    public Sprite spriteSauce;
    public Sprite spriteCheese;
    public Sprite spritePepperoni;
    public Sprite spriteVeggie;
    public Sprite spriteBaked;

    [Header("VFX Settings")]
    // --- [BARU] Slot untuk memasukkan Prefab VFX Debu ---
    public GameObject vfxDebuPrefab;

    [Header("Komponen UI")]
    public Button btnSauce;
    public Button btnCheese;
    public Button btnPepperoni;
    public Button btnVeggie;
    public Button btnBake;
    public Button btnSell;
    public Text textStatus;

    [Header("Audio Settings")]
    public AudioSource sourceSuara;
    public AudioClip sfxKaching;
    public AudioClip sfxBaking;

    private SpriteRenderer rend;
    private int doughClicks = 0;
    private bool isBaking = false;

    private enum State { Dough, Flat, Sauced, Cheesed, Pepperoni, Veggie, Baked }
    private State currentState;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        if (sourceSuara == null)
            sourceSuara = GetComponent<AudioSource>();

        ResetPizza();
    }

    // 1. Fungsi Klik Adonan
    void OnMouseDown()
    {
        if (currentState == State.Dough)
        {
            if (sourceSuara != null && sfxBaking != null)
            {
                sourceSuara.pitch = Random.Range(0.8f, 1.2f);
                sourceSuara.PlayOneShot(sfxBaking);
            }

            doughClicks++;

            // Efek visual (membal)
            transform.localScale = Vector3.one * 0.9f;
            Invoke("ResetScale", 0.1f);

            // Cek jika sudah diklik 3 kali
            if (doughClicks >= 3)
            {
                // --- [BARU] Munculkan VFX Debu di sini (Pas berubah jadi Flat) ---
                if (vfxDebuPrefab != null)
                {
                    Instantiate(vfxDebuPrefab, transform.position, Quaternion.identity);
                }
                // -------------------------------------------------------------

                currentState = State.Flat;
                rend.sprite = spriteFlat;

                if (sourceSuara != null) sourceSuara.pitch = 1f;

                UpdateUI();
            }
        }
    }

    void ResetScale() { transform.localScale = Vector3.one; }

    // ... (Sisa kode ke bawah TIDAK PERLU DIUBAH, biarkan sama persis seperti aslinya)

    public void AddSauce() { /* ... kode lama ... */ if (currentState == State.Flat) { currentState = State.Sauced; rend.sprite = spriteSauce; UpdateUI(); } }
    public void AddCheese() { /* ... kode lama ... */ if (currentState == State.Sauced) { currentState = State.Cheesed; rend.sprite = spriteCheese; UpdateUI(); } }
    public void AddPepperoni() { /* ... kode lama ... */ if (currentState == State.Cheesed) { currentState = State.Pepperoni; rend.sprite = spritePepperoni; UpdateUI(); } }
    public void AddVeggie() { /* ... kode lama ... */ if (currentState == State.Pepperoni) { currentState = State.Veggie; rend.sprite = spriteVeggie; UpdateUI(); } }
    public void BakePizza() { /* ... kode lama ... */ if (currentState == State.Veggie && !isBaking) { StartCoroutine(ProsesMasak()); } }
    IEnumerator ProsesMasak() { /* ... kode lama ... */ isBaking = true; btnBake.interactable = false; if (textStatus != null) textStatus.text = "Sedang Memanggang..."; yield return new WaitForSeconds(5f); currentState = State.Baked; isBaking = false; rend.sprite = spriteBaked; rend.color = Color.white; if (textStatus != null) textStatus.text = "MATANG! Siap Jual"; UpdateUI(); }
    public void JualPizza() { /* ... kode lama ... */ if (currentState == State.Baked) { if (sourceSuara != null && sfxKaching != null) { sourceSuara.pitch = 1f; sourceSuara.PlayOneShot(sfxKaching); } if (GameManager.instance != null) { GameManager.instance.TombolJualDitekan(); } ResetPizza(); } }
    void ResetPizza() { /* ... kode lama ... */ currentState = State.Dough; rend.sprite = spriteDough; rend.color = Color.white; doughClicks = 0; if (sourceSuara != null) sourceSuara.pitch = 1f; if (textStatus != null) textStatus.text = "Ketuk Adonan!"; UpdateUI(); }
    void UpdateUI() { /* ... kode lama ... */ btnSauce.interactable = false; btnCheese.interactable = false; btnPepperoni.interactable = false; btnVeggie.interactable = false; btnBake.interactable = false; btnSell.gameObject.SetActive(false); if (currentState == State.Flat) btnSauce.interactable = true; else if (currentState == State.Sauced) btnCheese.interactable = true; else if (currentState == State.Cheesed) btnPepperoni.interactable = true; else if (currentState == State.Pepperoni) btnVeggie.interactable = true; else if (currentState == State.Veggie) btnBake.interactable = true; else if (currentState == State.Baked) btnSell.gameObject.SetActive(true); }
}