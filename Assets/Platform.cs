using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Platform : Obstacle {
    private Bonus.BonusType _bonusType = Bonus.BonusType.BON_NONE;
    private void SetPlatformForLevelOfDifficulty() {
        _sprRend = GetComponent<SpriteRenderer>();
        switch (SceneManager.GetActiveScene().name) {
            case "1":

                break;
            case "2":
                _health = Mathf.CeilToInt(Random.Range(1, 4));
                _sprRend.color = gameMgr.instance().GetLifeColor(_health - 1);
                break;
            case "3":
                _health = Mathf.CeilToInt(Random.Range(1, 4));
                _sprRend.color = gameMgr.instance().GetLifeColor(_health - 1);
                
                if (Random.Range(0, 8) == 0) {
                    _bonusType = (Bonus.BonusType)Random.Range(1, 6);
                } 
                break;
        }
    }

    private void Start() {
        gameMgr.NumberOfPlatforms++;
        SetPlatformForLevelOfDifficulty();
    }

    private void OnDestroy() {
        gameMgr.NumberOfPlatforms--;
    }
    
    private void Update() {
        if (HitCheck()) {
            _health--;
            if (_health > 0) {
                _sprRend.color = gameMgr.instance().GetLifeColor(_health-1);
            } else {
                
                gameMgr.instance().UpdateScore();
                if (_bonusType != Bonus.BonusType.BON_NONE) {
                    var transform1 = transform;
                    gameMgr.instance().CreateBonusByBonusType(_bonusType,transform1.position,transform1.rotation);
                }
                Destroy(gameObject);
            }
        }
    }
}
