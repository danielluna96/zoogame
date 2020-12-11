using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deployBananas : MonoBehaviour
{
    public GameObject BananaColletiblePrefab;
    public float respawnTime = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bananaSpawn());
    }
    private void spawnBanana()
    {
        GameObject a = Instantiate(BananaColletiblePrefab) as GameObject;
        a.transform.position = new Vector2(7.33f, -11.43f);
    }
    IEnumerator bananaSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnBanana();
        }
    }
}
