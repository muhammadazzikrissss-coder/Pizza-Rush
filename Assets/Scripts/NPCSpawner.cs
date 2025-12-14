using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Masukkan Prefab NPC ke sini
    public float jedaWaktu = 2f; // Muncul setiap berapa detik?

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnNPC();
            timer = jedaWaktu; // Reset waktu
        }
    }

    void SpawnNPC()
    {
        // Munculkan NPC di posisi Spawner ini
        Instantiate(npcPrefab, transform.position, Quaternion.identity);
    }
}