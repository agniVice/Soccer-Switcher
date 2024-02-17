using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUserInterface : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Settings")]
    [SerializeField] private Button _soundToggle;
    [SerializeField] private Button _musicToggle;
    [SerializeField] private Button _vibrationToggle;

    [SerializeField] private Sprite _soundEnabled;
    [SerializeField] private Sprite _soundDisabled;

    [SerializeField] private Sprite _musicEnabled;
    [SerializeField] private Sprite _musicDisabled;

    [SerializeField] private Sprite _vibrationEnabled;
    [SerializeField] private Sprite _vibrationDisabled;

    [SerializeField] private List<Transform> _transformsMenu = new List<Transform>();
    [SerializeField] private List<Transform> _transformsSettings = new List<Transform>();

    [SerializeField] private Transform _star1;
    [SerializeField] private Transform _star2;
    [SerializeField] private Transform _star3;

    [SerializeField] private Transform _dropDownPanel;

    [SerializeField] private Image _currentLanguage;
    [SerializeField] private Sprite _english;
    [SerializeField] private Sprite _german;

    [SerializeField] private TextMeshProUGUI _highScoreText;

    private bool _dropDownOpened = false;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        AudioVibrationManager.Instance.SoundChanged += UpdateSoundImage;
        //AudioVibrationManager.Instance.SoundChanged += UpdateMusicImage;
        AudioVibrationManager.Instance.VibrationChanged += UpdateVibrationImage;

        _soundToggle.onClick.AddListener(AudioVibrationManager.Instance.ToggleSound);
        _soundToggle.onClick.AddListener(AudioVibrationManager.Instance.ToggleMusic);
        _vibrationToggle.onClick.AddListener(AudioVibrationManager.Instance.ToggleVibration);

        UpdateSoundImage();
        //UpdateMusicImage();
        UpdateVibrationImage();
        UpdateLanguageIcon();

        OnCloseSettingsButtonClicked();
    }
    private void OnDisable()
    {
        AudioVibrationManager.Instance.SoundChanged -= UpdateSoundImage;
        //AudioVibrationManager.Instance.SoundChanged -= UpdateMusicImage;
        AudioVibrationManager.Instance.VibrationChanged -= UpdateVibrationImage;
    }
    private void UpdateSoundImage()
    {
        if (AudioVibrationManager.Instance.IsSoundEnabled) 
            _soundToggle.GetComponent<Image>().sprite = _soundEnabled;
        else
            _soundToggle.GetComponent<Image>().sprite = _soundDisabled;
    }
    private void UpdateMusicImage()
    {
        if (AudioVibrationManager.Instance.IsMusicEnabled)
            _musicToggle.GetComponent<Image>().sprite = _musicEnabled;
        else
            _musicToggle.GetComponent<Image>().sprite = _musicDisabled;
    }
    private void UpdateVibrationImage()
    {
        if (AudioVibrationManager.Instance.IsVibrationEnabled)
            _vibrationToggle.GetComponent<Image>().sprite = _vibrationEnabled;
        else
            _vibrationToggle.GetComponent<Image>().sprite = _vibrationDisabled;
    }
    public void OnSettingsButtonClicked()
    {
        _menuPanel.SetActive(false);
        _settingsPanel.SetActive(true);

        foreach (Transform transform in _transformsSettings)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, Random.Range(0.2f, 0.7f)).SetEase(Ease.OutBack).SetLink(transform.gameObject).SetUpdate(true);
        }
    }
    public void OnCloseSettingsButtonClicked()
    {
        _menuPanel.SetActive(true);
        _settingsPanel.SetActive(false);

        _star1.localScale = Vector3.zero;
        _star2.localScale = Vector3.zero;
        _star3.localScale = Vector3.zero;

        _star1.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetLink(_star1.gameObject).SetDelay(0.1f);
        _star2.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetLink(_star2.gameObject).SetDelay(0.3f);
        _star3.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetLink(_star3.gameObject).SetDelay(0.5f);

        _highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        foreach (Transform transform in _transformsMenu)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, Random.Range(0.2f, 0.7f)).SetEase(Ease.OutBack).SetLink(transform.gameObject).SetUpdate(true);
        }
    }
    public void OnPlayButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnInputLanguage(int value)
    {
        LanguageManager.Instance.ChangeLanguage(value);
        DropDownInteract();
    }
    public void UpdateLanguageIcon()
    {
        if ((int)LanguageManager.Instance.CurrentLanguage == 0)
            _currentLanguage.sprite = _english;
        else
            _currentLanguage.sprite = _german;
    }
    public void DropDownInteract()
    {
        _dropDownOpened = !_dropDownOpened;
        _dropDownPanel.gameObject.SetActive(_dropDownOpened);
        UpdateLanguageIcon();
    }
    public void OnExitButtonClicked()
    { 
        Application.Quit();
    }
}
