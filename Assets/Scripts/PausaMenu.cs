using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PausaMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool juegoPausado = false;

    public GameObject pauseMenuUI;
    public GameObject contactosUI;
    public GameObject esquemaUI;

    public GameObject info_Ib, tatuaje_Ib;
    public GameObject info_Nanami, tatuaje_Nanami;
    public GameObject info_Ren, tatuaje_Ren;
    public GameObject info_Iwai, tatuaje_Iwai;
    public GameObject info_Izanagi, tatuaje_Izanagi;
    public GameObject info_Niko, tatuaje_Niko;

    Scene m_Scene;
    string sceneName;

    private PlayerInput pInput;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        pInput = Input.Instance.GetComponent<PlayerInput>();

        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("Player");

        pInput.currentActionMap.FindAction("Menu").performed += ctx => onPause();



        pInput.SwitchCurrentActionMap(actionMap);

     

        pauseMenuUI.SetActive(false);
        contactosUI.SetActive(false);
        esquemaUI.SetActive(false);
        tatuaje_Ib.SetActive(false);
        info_Ib.SetActive(false);
        tatuaje_Nanami.SetActive(false);
        info_Nanami.SetActive(false);
        tatuaje_Ren.SetActive(false);
        info_Ren.SetActive(false);
        tatuaje_Iwai.SetActive(false);
        info_Iwai.SetActive(false);
        tatuaje_Izanagi.SetActive(false);
        info_Izanagi.SetActive(false);
        tatuaje_Niko.SetActive(false);
        info_Niko.SetActive(false);
    }

    private void onPause(){
         if (juegoPausado) Resume();

         else Pause();
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        contactosUI.SetActive(false);
        esquemaUI.SetActive(false);
        tatuaje_Ib.SetActive(false);
        info_Ib.SetActive(false);
        tatuaje_Nanami.SetActive(false);
        info_Nanami.SetActive(false);
        tatuaje_Ren.SetActive(false);
        info_Ren.SetActive(false);
        tatuaje_Iwai.SetActive(false);
        info_Iwai.SetActive(false);
        tatuaje_Izanagi.SetActive(false);
        info_Izanagi.SetActive(false);
        tatuaje_Niko.SetActive(false);
        info_Niko.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void VolverJuego()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void VolverMenu()
    {
        pauseMenuUI.SetActive(true);
        contactosUI.SetActive(false);

    }

    public void VolverContactos()
    {
        contactosUI.SetActive(true);
        tatuaje_Ib.SetActive(false);
        info_Ib.SetActive(false);
        tatuaje_Nanami.SetActive(false);
        info_Nanami.SetActive(false);
        tatuaje_Ren.SetActive(false);
        info_Ren.SetActive(false);
        tatuaje_Iwai.SetActive(false);
        info_Iwai.SetActive(false);
        tatuaje_Izanagi.SetActive(false);
        info_Izanagi.SetActive(false);
        tatuaje_Niko.SetActive(false);
        info_Niko.SetActive(false);

    }

    public void Contactos()
    {
        pauseMenuUI.SetActive(false);
        contactosUI.SetActive(true);
        esquemaUI.SetActive(false);
    }

    public void Menu()
    {
        pauseMenuUI.SetActive(true);
        contactosUI.SetActive(false);
    }

    public void Esquema()
    {
        contactosUI.SetActive(false);
        esquemaUI.SetActive(true);

    }

    public void TatuajeIb()
    {
        contactosUI.SetActive(false);
        tatuaje_Ib.SetActive(true);
        info_Ib.SetActive(false);

    }

    public void InfoIb()
    {
        contactosUI.SetActive(false);
        tatuaje_Ib.SetActive(false);
        info_Ib.SetActive(true);

    }

    public void TatuajeNanami()
    {
        contactosUI.SetActive(false);
        tatuaje_Nanami.SetActive(true);
        info_Nanami.SetActive(false);

    }

    public void InfoNanami()
    {
        contactosUI.SetActive(false);
        tatuaje_Nanami.SetActive(false);
        info_Nanami.SetActive(true);

    }

    public void InfoRen()
    {
        contactosUI.SetActive(false);
        tatuaje_Ren.SetActive(false);
        info_Ren.SetActive(true);

    }
    public void TatuajeRen()
    {
        contactosUI.SetActive(false);
        tatuaje_Ren.SetActive(true);
        info_Ren.SetActive(false);

    }

    public void InfoIwai()
    {
        contactosUI.SetActive(false);
        tatuaje_Iwai.SetActive(false);
        info_Iwai.SetActive(true);

    }
    public void TatuajeIwai()
    {
        contactosUI.SetActive(false);
        tatuaje_Iwai.SetActive(true);
        info_Iwai.SetActive(false);

    }

    public void InfoIzanagi()
    {
        contactosUI.SetActive(false);
        tatuaje_Izanagi.SetActive(false);
        info_Izanagi.SetActive(true);

    }
    public void TatuajeIzanagi()
    {
        contactosUI.SetActive(false);
        tatuaje_Izanagi.SetActive(true);
        info_Izanagi.SetActive(false);

    }
    public void InfoNiko()
    {
        contactosUI.SetActive(false);
        tatuaje_Niko.SetActive(false);
        info_Niko.SetActive(true);

    }
    public void TatuajeNiko()
    {
        contactosUI.SetActive(false);
        tatuaje_Niko.SetActive(true);
        info_Niko.SetActive(false);

    }

    public void Reiniciar()
    {
        pauseMenuUI.SetActive(false);
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;

    }



}
