using UnityEngine;

public class NPCWalk : MonoBehaviour
{
    [Header("Settings")]
    public float kecepatan = 2f;
    public float jarakAntrian = 1.5f; // Jarak aman antrian
    public LayerMask layerTeman;      // Layer khusus NPC

    [Header("Components")]
    public GameObject orderBubble;

    private Rigidbody2D rb;
    private bool sedangPesan = false;
    private Collider2D myCollider; // Collider sendiri biar gak kena scan

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); // Ambil collider sendiri

        if (orderBubble != null) orderBubble.SetActive(false);
    }

    void FixedUpdate()
    {
        // --- PERBAIKAN DI SINI ---

        // Kita geser titik mulai laser sedikit ke kanan (misal 0.5 unit)
        // Sesuaikan angka 0.5f ini dengan lebar spritemu
        Vector2 posisiAwal = (Vector2)transform.position + (Vector2.right * 0.5f);
        Vector2 arah = Vector2.right;

        // Tembak laser
        RaycastHit2D hit = Physics2D.Raycast(posisiAwal, arah, jarakAntrian, layerTeman);

        // Gambar garis debug (Merah = Laser) biar kelihatan di Scene
        Debug.DrawRay(posisiAwal, arah * jarakAntrian, Color.red);

        bool adaTeman = false;

        if (hit.collider != null)
        {
            // Karena start laser sudah dimajukan, kita gak perlu cek diri sendiri lagi
            // Asalkan yang kena itu Layer NPC, berarti itu teman
            adaTeman = true;
        }

        // --- AKHIR PERBAIKAN ---

        // Logika Gerak (Sama seperti sebelumnya)
        if (sedangPesan || adaTeman)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = new Vector2(kecepatan, rb.linearVelocity.y);
        }
    }

    // ... (kode atas tetap sama) ...

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

    // ... (Sisa kode tetap sama) ...
}