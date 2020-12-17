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

#if UNITY_WEBGL
        video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cinematica.mp4");
#endif

        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(NextScene);
    }
}
