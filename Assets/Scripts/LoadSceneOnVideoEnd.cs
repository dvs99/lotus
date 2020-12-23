using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoadSceneOnVideoEnd : MonoBehaviour
{
    private VideoPlayer video;
    private PlayerInput pInput;
    [SerializeField] private Image img=null;
    public string NextScene;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        pInput = GetComponent<PlayerInput>();

#if UNITY_WEBGL
        video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cinematica.mp4");
#endif
    }

    private void Start()
    {
        pInput.currentActionMap.FindAction("Interact").performed += ctx => OnButton(ctx);
    }

    public void OnButton(InputAction.CallbackContext context)
    {
        if (img != null)
        {
            img.enabled = false;
            video.Play();
            video.loopPointReached += CheckOver;
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(NextScene);
    }
}
