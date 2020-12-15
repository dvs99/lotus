using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadSceneOnVideoEnd : MonoBehaviour
{
    private VideoPlayer video;
    public string NextScene;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(NextScene);
    }
}
