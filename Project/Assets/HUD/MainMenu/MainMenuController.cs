using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument document;
    private FMOD.Studio.VCA masterVcaController;
    private FMOD.Studio.VCA musicVcaController;
    private FMOD.Studio.VCA environmentVcaController;
    private FMOD.Studio.VCA effectsVcaController;

    private Button playButton;
    private Button settingsButton;
    private Button exitButton;

    private VisualElement settingsContainer;
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider environmentSlider;
    private Slider effectsSlider;

    // Start is called before the first frame update
    void Start()
    {
        document = GetComponent<UIDocument>();

        VisualElement root = document.rootVisualElement;
        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        exitButton = root.Q<Button>("ExitButton");
        settingsContainer = root.Q<VisualElement>("SettingsContainer");
        masterSlider = root.Q<Slider>("MasterSlider");
        musicSlider = root.Q<Slider>("MusicSlider");
        environmentSlider = root.Q<Slider>("EnvironmentSlider");
        effectsSlider = root.Q<Slider>("EffectsSlider");

        settingsButton.clicked += () =>
        {
            settingsContainer.ToggleInClassList("fadeOut");
            settingsButton.ToggleInClassList("active");
        };
        exitButton.clicked += () => Application.Quit();

        masterVcaController = FMODUnity.RuntimeManager.GetVCA("vca:/master");
        musicVcaController = FMODUnity.RuntimeManager.GetVCA("vca:/music");
        environmentVcaController = FMODUnity.RuntimeManager.GetVCA("vca:/environment");
        effectsVcaController = FMODUnity.RuntimeManager.GetVCA("vca:/effects");

        masterSlider.RegisterValueChangedCallback(value => masterVcaController.setVolume(value.newValue));
        musicSlider.RegisterValueChangedCallback(value => musicVcaController.setVolume(value.newValue));
        environmentSlider.RegisterValueChangedCallback(value => environmentVcaController.setVolume(value.newValue));
        effectsSlider.RegisterValueChangedCallback(value => effectsVcaController.setVolume(value.newValue));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
