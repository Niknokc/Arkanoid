using UnityEngine;

public class Ball : MonoBehaviour {
    private Vector3 _baseScale;
    private Vector3 _platformScale;
    private SpriteRenderer _sprRend;
    private float _speed = 5.0f;
    private Vector3 _direction;
    private int id = 0;
    private static int _currentId = 0;
    private static bool _isSpedUp;
    private const float SpeedUpModifier = 1.5f;
    private static bool IsSlowedDown;
    private const float SlowDownModifier = 0.75f;
    private static float SpeedModifier = 1;
    private static float SpeedTimer = 0;
    private const float SpeedTimeMax = 30;
    private bool _stopped=false;
    public bool attached = true;

    public Vector3 GetPlatformScale() {
        return _platformScale;
    }
    
    public SpriteRenderer GetSpriteRenderer() {
        return _sprRend;
    }

    public void SetDirection(Vector3 direction) {
        _direction = direction;
    }

    public float GetSpeed() {
        return _speed;
    }

    //public methods
    
    public void CreateADouble() {
        var transform1 = transform;
        Ball newBall = Instantiate(this, transform1.position, transform1.rotation);
        newBall.transform.Rotate(0,0,135f);
        newBall.attached = false;
        newBall._baseScale = _baseScale;
        newBall._platformScale = _platformScale;
        _currentId++;
        newBall.id = _currentId;
        gameMgr.instance().AddBall(newBall);
    }

    public void StopTheBall() {
        _stopped = true;
    }
    
    public void ChangeID() {
        id = 0;
        _currentId = 0;
    }
    public void Drop() {
        attached = false;
        var transform1 = transform;
        transform1.parent = null;
        transform1.localScale = _baseScale;
    }

    public void RotateTheBall(Vector3 targRot) {
        transform.up = targRot;
    }

    public void TurnAround() {
        transform.Rotate(0,0,180);
    }
    
    public static void SpeedTheBallUp() {
        if (_isSpedUp) {
            return;
        }
        if (IsSlowedDown) {
            IsSlowedDown = false;
        } else {
            _isSpedUp = true;
            SpeedTimer = SpeedTimeMax;
        }
        SpeedModifier = SpeedUpModifier;
    }

    public static void SlowTheBallDown() {
        if (IsSlowedDown) {
            return;
        }
        if (_isSpedUp) {
            _isSpedUp = false;
        } else {
            IsSlowedDown = true;
            SpeedTimer = SpeedTimeMax;
        }

        SpeedModifier = SlowDownModifier;
    }
    
    //Monobehaviour methods
    
    private void Start() {
        _direction = Vector3.up;
        var transform1 = transform;
        _baseScale = transform1.lossyScale;
        _platformScale = transform1.localScale;
        _sprRend = GetComponent<SpriteRenderer>();
    }
    
    private void Update() {
        if (_isSpedUp || IsSlowedDown) {
            SpeedTimer -= Time.deltaTime;
            if (SpeedTimer < 0) {
                if (_isSpedUp) {
                    _isSpedUp = false;
                }

                if (IsSlowedDown) {
                    IsSlowedDown = false;
                }

                SpeedModifier = 1;
            }
        }
        Ray ray = new Ray(transform.position,transform.up);
        gameMgr.instance().SetRay(ray,id);
    }

    void FixedUpdate(){
        if (!attached && !_stopped) {
            transform.Translate(_speed*_direction * SpeedModifier * Time.fixedDeltaTime);
        }
    }
    
    void OnBecameInvisible() {
        if (gameMgr.instance() != null) {
            gameMgr.instance().BallFallenOff(id);
        }
    }
}
