
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class gameMgr : MonoBehaviour {
    
    private static gameMgr GameMgr;
    [SerializeField]
    private List<Ball> _balls;
    
    private List<Ray> _rays = new List<Ray>();
   
    
    [FormerlySerializedAs("scoreText")] [SerializeField]
    private Text _scoreText;
    private int _score = 0;
    
    [FormerlySerializedAs("lifeColors")] [SerializeField]
    private List<Color> _lifeColors = new List<Color>();
    [FormerlySerializedAs("liveImage")] [SerializeField]
    private GameObject _liveImage;
    private List<GameObject> _lives = new List<GameObject>();
   
    private byte _livesMax = 3;
    private byte _currentLives = 0;
    
    [FormerlySerializedAs("gamePlatform")] [SerializeField]
    private GamePlatform _gamePlatform;
    private Vector3 _startingPos;

    private bool isCongratulationsOn = false;
    public bool ballFallen = false;
    public Bonus bonusTemplate;
    
    public static int NumberOfPlatforms = 0;
    
    [SerializeField]
    private Canvas _canvas;
    
    [FormerlySerializedAs("gameOverScreen")] [SerializeField]
    private GameObject _gameOverScreen;
    [FormerlySerializedAs("congratsScreen")] [SerializeField]
    private GameObject _congratsScreen;
    
    [SerializeField]
    private AudioMgr _audioMgr;

    //Setters /Getters
    
    public void SetBallDirection(Vector3 direction, int id) {
        _balls[id].SetDirection(direction);
    }
    
    public GamePlatform GetGamePlatform() {
        return _gamePlatform;
    }
    
    public Ball GetBall(int id) {
        if (_balls.Count > id) {
            
            return _balls[id];
        }
        else {
            return null;
        }
    }
    
    public Color GetLifeColor(int i) {
        return _lifeColors[i];
    }
    public void SetRay(Ray newRay, int id) {
        if (_rays.Count == id ) {
            _rays.Add(newRay);
        } else {
            if (_rays.Count >= id) {
                _rays[id] = newRay;
            }
        }
    }

    public Ray GetRay(int id) {
        return _rays[id];
    }

    public int GetNumberOfRays() {
        return _rays.Count;
    }
    
    public int GetScore() {
        return _score;
    }
    //Public methods
    public void CreateBonusByBonusType(Bonus.BonusType bonusType,Vector3 position, Quaternion rotation) {
        Bonus bonusGO = Instantiate(bonusTemplate, position, rotation);
        bonusGO.SetBonusType(bonusType);
        bonusGO.gameObject.SetActive(true);
    }

    public void AddBall(Ball newBall) {
        _balls.Add(newBall);
        print("?");
    }

    public void DropTheBall() {
        _balls[0].Drop();
        ballFallen = false;
    }
    public void StartGame() {
        _startingPos = _balls[0].transform.localPosition;
        _currentLives = _livesMax;
        _audioMgr.PlayStartSound();
        for (int i = 0; i < _currentLives; i++) {
            GameObject img = Instantiate(_liveImage,_liveImage.transform.parent);
            _lives.Add(img);
            img.SetActive(true);
        }
        DropTheBall();
    }
    
    public void BallHit(int id) {
        _balls[id].TurnAround();
        _audioMgr.PlayPopSound();
    }
    
    public void BallHit(Vector3 targetRot,int id) {
        _balls[id].RotateTheBall(targetRot);
        _audioMgr.PlayPopSound();
    }
    
    

    public void UpdateScore() {
        _score += 20;
        _scoreText.text = _score.ToString();
    }

    public void BallFallenOff(int id) {
        if (_balls.Count <= 1) {
            _currentLives--;
            if (_currentLives > 0 && _gamePlatform!=null && _gamePlatform.isActiveAndEnabled  && _balls!=null && _balls.Count>id && _balls[id]!=null) {
                _audioMgr.PlayDropSound();
                ballFallen = true;
                _balls[id].attached = true;
                _balls[id].transform.parent = _gamePlatform.transform;
                _balls[id].transform.localPosition = _startingPos;
                _balls[id].transform.eulerAngles = Vector3.zero;
                _balls[id].transform.localScale = _balls[id].GetPlatformScale();
                
                _balls[id].ChangeID();
                if (_lives!=null && _lives.Count>=_currentLives && _lives[_currentLives] != null ) {
                    Destroy(_lives[_currentLives]);
                }
            } else {
                ShowGameOverScreen();
            }
        } else {
            Destroy(_balls[id].gameObject);
            _balls.Remove(_balls[id]);
            if (_rays.Count>=id) {
                _rays.Remove(_rays[id]);
            }
            _balls[0].ChangeID();
            
        }
    }

    public void PlayBonusSound() {
        _audioMgr.PlayBonusSound();
    }
    
    private void ShowGameOverScreen() {
        _audioMgr.PlayLoseSound();
        Instantiate(_gameOverScreen, _canvas.transform);
    }

    public static gameMgr instance() {
        return GameMgr;
    }

    //MonoBehaviours
    
    private void Awake() {
        if (GameMgr == null) {
            GameMgr = this;
        }
    }
    
    private void Update(){
        if (NumberOfPlatforms <= 0 && !isCongratulationsOn) {
            Instantiate(_congratsScreen, _canvas.transform);
            _audioMgr.PlayWinSound();
            isCongratulationsOn = true;
            _balls[0].attached = true;
            _balls[0].StopTheBall();
        }
    }
}
