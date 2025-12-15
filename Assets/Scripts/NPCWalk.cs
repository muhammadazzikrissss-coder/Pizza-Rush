using UnityEngine;

public class NPCWalk : MonoBehaviour
{
    [Header("Settings")]
    public float kecepatan = 2f;
    public float jarakAntrian = 1.5f;
    public LayerMask layerTeman;

    [Header("Components")]
    public GameObject orderBubble;
    private Rigidbody2D rb;
    private Animator anim; // <-- TAMBAHAN 1: Variabel Animator

    private bool sedangPesan = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // <-- TAMBAHAN 2: Ambil komponen Animator

        if (orderBubble != null) orderBubble.SetActive(false);
    }

    void FixedUpdate()
    {
        // ... (KODE RAYCAST MAJU SEDIKIT YANG KEMARIN TETAP DISINI) ...
        Vector2 posisiAwal = (Vector2)transform.position + (Vector2.right * 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(posisiAwal, Vector2.right, jarakAntrian, layerTeman);
        bool adaTeman = (hit.collider != null);
        // ...

        // Logika Gerak
        if (sedangPesan || adaTeman)
        {
            rb.linearVelocity = Vector2.zero; // Stop
        }
        else
        {
            rb.linearVelocity = new Vector2(kecepatan, rb.linearVelocity.y); // Jalan
        }

        // --- TAMBAHAN 3: Update Animasi berdasarkan kecepatan ---
        // Kalau kecepatan horizontalnya lebih dari 0.1 (artinya bergerak)
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            anim.SetBool("isWalking", true); // Kirim sinyal jalan ke Animator
        }
        else
        {
            anim.SetBool("isWalking", false); // Kirim sinyal diam
        }
        // ---------------------------------------------------------
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Cek Trigger Toko
        if (other.CompareTag("Toko"))
        {
            // Masuk mode antri/pesan
            sedangPesan = true;
            if (orderBubble != null) orderBubble.SetActive(true);

            // JANGAN PAKAI INVOKE LAGI.
            // Tapi lapor ke Bos (GameManager) kalau saya sudah sampai
            GameManager.instance.LaporAdaCustomer(this);
        }
    }

    // Fungsi baru: Dipanggil oleh GameManager saat tombol JUAL ditekan
    public void PesananSelesai()
    {
        sedangPesan = false; // Boleh jalan lagi

        if (orderBubble != null)
            orderBubble.SetActive(false); // Tutup bubble chat

        // Opsional: Matikan collider sebentar biar gak dideteksi teman di belakang
        // Tapi logic Raycast kita sebelumnya sudah cukup aman kok.
    }
}