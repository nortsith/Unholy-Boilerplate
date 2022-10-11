using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

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

    Resolution[] resolutions;
    List<string> refreshRates;
    private DropdownField modeDropdown;
    private DropdownField resolutionDropdown;
    private DropdownField refreshRateDropdown;

    private DropdownField qualityDropdown;

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

        modeDropdown = root.Q<DropdownField>("ModeDropdown");
        resolutionDropdown = root.Q<DropdownField>("ResolutionDropdown");
        refreshRateDropdown = root.Q<DropdownField>("RefreshRateDropdown");
        PopulateModes();
        PopulateResolutions();
        PopulateRefreshRates();

        qualityDropdown = root.Q<DropdownField>("QualityDropdown");
        PopulateQualityOptions();
    }

    private void PopulateModes()
    {
        modeDropdown.choices = new List<string>() { "Fullscreen", "Borderless", "Windowed" };
        modeDropdown.index = 0;

        modeDropdown.RegisterValueChangedCallback(value => SetMode());
    }

    private void SetMode()
    {
        switch (modeDropdown.value)
        {
            case "Fullscreen":
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case "Borderless":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case "Windowed":
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    private void PopulateResolutions()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int index = 0; index < resolutions.Length; index++)
        {
            string option = resolutions[index].width + "x" + resolutions[index].height;
            options.Add(option);

            if (resolutions[index].width == Screen.currentResolution.width &&
                resolutions[index].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = index;
            }
        }

        resolutionDropdown.choices = options;
        resolutionDropdown.index = currentResolutionIndex;

        resolutionDropdown.RegisterValueChangedCallback(value => SetResolution());
    }

    private void SetResolution()
    {
        Resolution resolution = resolutions[resolutionDropdown.index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void PopulateRefreshRates()
    {
        refreshRates = new List<string>() { "Uncapped", "60", "144", "240" };
        int currentRefreshRateIndex = 0;
        refreshRateDropdown.choices = refreshRates;
        refreshRateDropdown.index = currentRefreshRateIndex;
        refreshRateDropdown.RegisterValueChangedCallback(value => SetRefreshRate());
    }

    private void SetRefreshRate()
    {
        if (refreshRateDropdown.value == "Uncapped")
            Application.targetFrameRate = -1;
        else
            Application.targetFrameRate = int.Parse(refreshRateDropdown.value);
    }

    private void PopulateQualityOptions()
    {
        qualityDropdown.choices = new List<string>() { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };
        qualityDropdown.RegisterValueChangedCallback(value => SetQuailty());
        qualityDropdown.index = QualitySettings.GetQualityLevel();
    }

    private void SetQuailty()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.index);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
