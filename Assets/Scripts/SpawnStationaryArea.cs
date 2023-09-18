using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStationaryArea : MonoBehaviour
{
    public List<Rect> areaToSpawnFrom;
    public List<Rect> areaToExcludeFrom;

    public Color colorShade;

    public GameObject objToSpawn;
    
    public float spawnFrequency = 1f;

    private float timeDuration;
    private float timer = 0f;

    private void Start() {
        timeDuration = 1f / spawnFrequency;
    }

    private void Update()
    {
        Spawn();
    }

    private void Spawn(){
        timer += Time.deltaTime;
        if (timer < timeDuration){
            return;
        }

        timer = timer % timeDuration;

        // Ensure that object will spawn within the box of the spawning area

        int position = Random.Range(0, areaToSpawnFrom.Count);
        Rect roughArea = areaToSpawnFrom[position];
        Vector2 objSize = objToSpawn.GetComponent<SpriteRenderer>().bounds.size;
        
        float xComponent = Mathf.Round(roughArea.xMin + Random.Range(0, roughArea.width));
        xComponent = xComponent <= roughArea.xMin ? roughArea.xMin + objSize.x/2 : xComponent;
        xComponent = xComponent >= roughArea.xMax ? roughArea.xMax - objSize.x/2 : xComponent;

        float yComponent = Mathf.Round(roughArea.yMin + Random.Range(0, roughArea.height));
        yComponent = yComponent <= roughArea.yMin ? roughArea.yMin + objSize.y/2 : yComponent;
        yComponent = yComponent >= roughArea.yMax ? roughArea.yMax - objSize.y/2 : yComponent;
        Vector2 spawnPosition = new Vector2(xComponent, yComponent);

        Instantiate(objToSpawn, spawnPosition, Quaternion.identity);     
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = colorShade;
        foreach (Rect area in areaToSpawnFrom){
            Gizmos.DrawCube(area.center, new Vector2(area.width, area.height));
        }        
    }
}
