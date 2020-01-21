using UnityEngine;
using UnityEngine.Serialization;

public class GamePlatform : Obstacle
{
    [FormerlySerializedAs("leftBlocker")] [SerializeField] 
    private Transform _leftBlocker;
    [FormerlySerializedAs("rightBlocker")] [SerializeField] 
    private Transform _rightBlocker;
    private bool _gameOn = false;
    [FormerlySerializedAs("speed")] [SerializeField]
    private float _speed;

    private bool _isEnlarged = false;
    
    private bool _isShrunk = false;

    private float _sizeModifier;

    private float _sizeTimer = 0;
    private const float KSizeTimeMax = 30;
    
    public void EnlargePlatform() {
        if (_isEnlarged) {
            return;
        }
        if (_isShrunk) {
            _isShrunk = false;
        } else {
            _isEnlarged = true;
            _sizeTimer = KSizeTimeMax;
        }
        transform.localScale = new Vector3(transform.localScale.x*1.5f,transform.localScale.y,1);
    }

    public void ShrinkPlatform() {
        if (_isShrunk) {
            return;
        }
        if (_isEnlarged) {
            _isEnlarged = false;
        } else {
            _isShrunk = true;
            _sizeTimer = KSizeTimeMax;
        }
        
        transform.localScale = new Vector3(transform.localScale.x/1.5f,transform.localScale.y,1);
    }
    
    new void HitCheck() {
        for (int i = 0; i < gameMgr.instance().GetNumberOfRays(); i++) {
            if (_justHit.Count <= i) {
                _justHit.Add(false);
                _coolTimer.Add(10);
            }
            float distance = 0;
            Ray ray = gameMgr.instance().GetRay(i);
            if (_sprRend.bounds.Intersects(gameMgr.instance().GetBall(i).GetSpriteRenderer().bounds)) {
                if (distance < _sprRend.bounds.extents.y && !_justHit[i]) {
                    _justHit[i] = true;
                    Vector3 closestPoint = _sprRend.bounds.ClosestPoint(ray.origin);
                    closestPoint[1] /= 2;
                    Vector3 targetRotation = closestPoint - transform.position;
                    gameMgr.instance().BallHit(targetRotation,i);
                }
            } else {
                _justHit[i] = false;
            }
        }
    }
    
    public SpriteRenderer GetSpriteRenderer() {
        return _sprRend;
    }
    
    // Update is called once per frame
    void Update() {
        if (_isEnlarged || _isShrunk) {
            _sizeTimer -= Time.deltaTime;
            if (_sizeTimer < 0) {
                if (_isEnlarged) {
                    ShrinkPlatform();
                }

                if (_isShrunk) {
                    EnlargePlatform();
                }
            }
        }
        
        if (_gameOn) {
            HitCheck();
        }
        
        if (Input.GetKey(KeyCode.Space)) {
            if (!_gameOn) {
                gameMgr.instance().StartGame();
                _gameOn = true;
            } else if(gameMgr.instance().ballFallen) {
                gameMgr.instance().DropTheBall();
            }
        } 
        
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.A)) {
            if (_leftBlocker.position.x < transform.position.x - _sprRend.bounds.extents.x) {
                transform.Translate(-transform.right * Time.fixedDeltaTime * _speed,Space.World);
            }
        }
        if (Input.GetKey(KeyCode.D)) {

            if (_rightBlocker.position.x > transform.position.x + _sprRend.bounds.extents.x) {
                transform.Translate(transform.right * Time.fixedDeltaTime * _speed, Space.World);
            }
        }
    }
}
