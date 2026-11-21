using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMenu;
    public GameObject panelOpciones;
    public GameObject panelCreditos;

    [Header("Audio")]
    public AudioMixer mixer;
    public Slider sliderMaster;
    public Slider sliderMusica;
    public Slider sliderSFX;

    private void Start()
    {
        // Arranca mostrando solo el menú principal
        VolverAlMenu();

        // Carga volúmenes guardados
        float master = PlayerPrefs.GetFloat("MasterVol", 0.8f);
        float musica = PlayerPrefs.GetFloat("BGMVol", 0.8f);
        float sfx = PlayerPrefs.GetFloat("SFXVol", 0.8f);

        if (sliderMaster != null) sliderMaster.value = master;
        if (sliderMusica != null) sliderMusica.value = musica;
        if (sliderSFX != null) sliderSFX.value = sfx;

        CambiarVolumenMaster(master);
        CambiarVolumenMusica(musica);
        CambiarVolumenSFX(sfx);
    }

    // --- BOTONES DEL MENÚ ---
    public void Jugar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level01"); // Asegurate que la escena se llame así
    }

    public void AbrirOpciones()
    {
        panelMenu.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void AbrirCreditos()
    {
        panelMenu.SetActive(false);
        panelCreditos.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelOpciones.SetActive(false);
        panelCreditos.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    // --- SLIDERS DE AUDIO ---
    public void CambiarVolumenMaster(float valor)
    {
        float v = Mathf.Clamp(valor, 0.0001f, 1f);
        mixer.SetFloat("MasterVol", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("MasterVol", v);
    }

    public void CambiarVolumenMusica(float valor)
    {
        float v = Mathf.Clamp(valor, 0.0001f, 1f);
        mixer.SetFloat("BGMVol", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("BGMVol", v);
    }

    public void CambiarVolumenSFX(float valor)
    {
        float v = Mathf.Clamp(valor, 0.0001f, 1f);
        mixer.SetFloat("SFXVol", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("SFXVol", v);
    }
}