using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    // Hapus object setelah 1 detik (sesuaikan dengan durasi animasimu)
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}