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
    public AudioClip sfxKaching;    // Suara Uang
    public AudioClip sfxBaking;     // --- TAMBAHAN BARU: Suara Adonan ---

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
            // --- TAMBAHAN BARU: Mainkan Suara Tumbuk ---
            if (sourceSuara != null && sfxBaking != null)
            {
                // Kita random sedikit pitch-nya biar suaranya tidak monoton kalau diklik cepat
                sourceSuara.pitch = Random.Range(0.8f, 1.2f);
                sourceSuara.PlayOneShot(sfxBaking);
            }
            // -------------------------------------------

            doughClicks++;

            // Efek visual
            transform.localScale = Vector3.one * 0.9f;
            Invoke("ResetScale", 0.1f);

            if (doughClicks >= 3)
            {
                currentState = State.Flat;
                rend.sprite = spriteFlat;

                // Balikin pitch ke normal setelah adonan jadi gepeng
                if (sourceSuara != null) sourceSuara.pitch = 1f;

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

    // 3. Fungsi Bake
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

        yield return new WaitForSeconds(5f);

        currentState = State.Baked;
        isBaking = false;
        rend.sprite = spriteBaked;
        rend.color = Color.white;

        if (textStatus != null) textStatus.text = "MATANG! Siap Jual";
        UpdateUI();
    }

    // 4. Fungsi Jual
    public void JualPizza()
    {
        if (currentState == State.Baked)
        {
            if (sourceSuara != null && sfxKaching != null)
            {
                // Pastikan pitch normal untuk suara uang
                sourceSuara.pitch = 1f;
                sourceSuara.PlayOneShot(sfxKaching);
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.TombolJualDitekan();
            }

            ResetPizza();
        }
    }

    void ResetPizza()
    {
        currentState = State.Dough;
        rend.sprite = spriteDough;
        rend.color = Color.white;
        doughClicks = 0;

        // Reset pitch jaga-jaga
        if (sourceSuara != null) sourceSuara.pitch = 1f;

        if (textStatus != null) textStatus.text = "Ketuk Adonan!";
        UpdateUI();
    }

    void UpdateUI()
    {
        btnSauce.interactable = false;
        btnCheese.interactable = false;
        btnPepperoni.interactable = false;
        btnVeggie.interactable = false;
        btnBake.interactable = false;
        btnSell.gameObject.SetActive(false);

        if (currentState == State.Flat) btnSauce.interactable = true;
        else if (currentState == State.Sauced) btnCheese.interactable = true;
        else if (currentState == State.Cheesed) btnPepperoni.interactable = true;
        else if (currentState == State.Pepperoni) btnVeggie.interactable = true;
        else if (currentState == State.Veggie) btnBake.interactable = true;
        else if (currentState == State.Baked) btnSell.gameObject.SetActive(true);
    }
}