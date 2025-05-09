using UnityEngine;

public class RiverSoundController : MonoBehaviour
{
    public Transform player; 
    public AudioSource audioSource; 
    public float maxDistance = 50f; 
    private Transform closestWater; 

    void Update()
    {
        closestWater = GetClosestWaterTile();

        if (closestWater != null)
        {
            float distance = Vector3.Distance(player.position, closestWater.position);

            float volume = Mathf.Clamp01(1 - (distance / maxDistance));

            audioSource.volume = Mathf.Clamp01(volume * 0.1f);
        }
    }

    Transform GetClosestWaterTile()
    {
        GameObject[] waterTiles = GameObject.FindGameObjectsWithTag("Water");
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject waterTile in waterTiles)
        {
            float distance = Vector3.Distance(player.position, waterTile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = waterTile.transform;
            }
        }
        return closest;
    }
}