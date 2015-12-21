using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Music : MonoBehaviour
{
    AudioSource audioSource;
    string[] playList;
    public string fullPathToMusicFolder = @"Assets\Resources\Music";
     
    private AudioClip currentSong;
    private AudioClip nextSong;
    private int nextSongIndex = 0;
    private ResourceRequest nextSongRequest;

    // Use this for initialization
    void Start()
    {
        //Collect all song files from Music dir
        var musicDir = new DirectoryInfo(fullPathToMusicFolder);
        var musicFiles = musicDir.GetFiles();
        var tempList = new List<string>();
        foreach (FileInfo file in musicFiles)
        {
            if (file.Extension != ".meta")
            {
                tempList.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
        }

        playList = tempList.ToArray();
        audioSource = GetComponent<AudioSource>();
        currentSong = Resources.Load<AudioClip>(GetNextSongPath());
        audioSource.PlayOneShot(currentSong);
        nextSongRequest = Resources.LoadAsync<AudioClip>(GetNextSongPath());
        StartCoroutine(LoadNextSong());
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (nextSong == null)
                print("Next track failed to load properly.");

            currentSong = nextSong;
            audioSource.PlayOneShot(currentSong);

            nextSongRequest = Resources.LoadAsync<AudioClip>(GetNextSongPath());
            StartCoroutine(LoadNextSong());            
        }
    }

    private string GetNextSongPath()
    {
        //if at the start of the playlist, shuffle it
        if(nextSongIndex == 0)
        {
            for (var i = 0; i < playList.Length; i++)
            {
                int j = Random.Range(i, playList.Length);
                var temp = playList[i];
                playList[i] = playList[j];
                playList[j] = temp;
            }
        }

        string path = "Music\\" + playList[nextSongIndex];
        nextSongIndex = nextSongIndex == playList.Length - 1 ? 0 : ++nextSongIndex;

        return path;
    }

    IEnumerator LoadNextSong()
    {
        yield return new WaitUntil(NextSongRequestLoaded);

        nextSong = (AudioClip)nextSongRequest.asset;
    }

    bool NextSongRequestLoaded()
    {
        return nextSongRequest.isDone;
    }
}
