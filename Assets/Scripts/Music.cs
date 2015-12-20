using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Music : MonoBehaviour
{
    AudioSource audioSource;
    List<string> playList;
    public string fullPathToMusicFolder = @"Assets\Resources\Music";
    private AudioClip nextSong;
    private AudioClip currentSong;

    // Use this for initialization
    void Start()
    {
        playList = new List<string>();
        BuildPlayList();

        audioSource = GetComponent<AudioSource>();
        currentSong = Resources.Load<AudioClip>(GetRandomSongPathFromPlayList());
        audioSource.PlayOneShot(currentSong);

        print(Time.time);
        StartCoroutine(LoadNextSong());
        print(Time.time);
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

            print("1:" + Time.time);
            StartCoroutine(LoadNextSong());
            print("1" + Time.time);
        }
    }

    private string GetRandomSongPathFromPlayList()
    {
        int index = Random.Range(0, playList.Count);

        var songName = playList[index];
        playList.RemoveAt(index);

        if (playList.Count == 0)
            BuildPlayList();

        return "Music\\" + songName;
    }

    private void BuildPlayList()
    {
        var info = new DirectoryInfo(fullPathToMusicFolder);
        var fileInfo = info.GetFiles();

        foreach (FileInfo f in fileInfo)
        {
            if (!f.Name.Contains("meta"))
            {
                var fileNameNoExtension = Path.GetFileNameWithoutExtension(f.FullName);
                playList.Add(fileNameNoExtension);
            }
        }
    }

    IEnumerator LoadNextSong()
    {
        var res = Resources.LoadAsync<AudioClip>(GetRandomSongPathFromPlayList());

        while (!res.isDone)
        {
            print(res.isDone);
            yield return null;
        }

        nextSong = (AudioClip)res.asset;
    }


}
