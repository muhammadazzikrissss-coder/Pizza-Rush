using UnityEngine;

public class DestroyerWall : MonoBehaviour
{
    // Fungsi ini terpanggil kalau ada sesuatu yang masuk ke area trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang nabrak punya tag "Player" atau bukan
        // Biar aman, hancurkan object apapun yang nabrak tembok ini
        Destroy(other.gameObject);
    }
}