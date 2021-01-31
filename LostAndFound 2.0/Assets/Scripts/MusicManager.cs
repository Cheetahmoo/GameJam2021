using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource ambientMusic;
    public AudioSource killMusic;
    public EnemySpawner spawner;
    public bool musicState;
    public float fadeTime;
    float currTime;
    // Start is called before the first frame update
    void Start()
    {
        ambientMusic.volume = 0;
        killMusic.volume = 0;
        ambientMusic.Play();
        killMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(currTime <= fadeTime)
        {
            currTime += Time.deltaTime;
        }

        if(spawner.livingEnemies.Count != 0)
        {
            if (!musicState)
            {
                currTime = 0;
            }
            musicState = true;
        }
        else
        {
            if (musicState)
            {
                currTime = 0;
            }
            musicState = false;
        }

        if (musicState)
        {
            killMusic.volume = Mathf.Lerp(killMusic.volume, 1, currTime / fadeTime);
            ambientMusic.volume = Mathf.Lerp(ambientMusic.volume, 0, currTime / fadeTime);
        }
        else
        {
            killMusic.volume = Mathf.Lerp(killMusic.volume, 0, currTime / fadeTime);
            ambientMusic.volume = Mathf.Lerp(ambientMusic.volume, 1, currTime / fadeTime);
        }
        
    }
}
