using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public enum BonusType {
        BON_NONE = 0,
        BON_BALLS = 1,
        BON_SHRUNK = 2,
        BON_ENLARGE = 3,
        BON_SPEEDUP = 4,
        BON_SLOWDOWN = 5
    }

    public List<Sprite> bonusImages;
    [SerializeField]
    private BonusType _bonusType;
    [SerializeField]
    private SpriteRenderer _sprRend;
    
    public void SetBonusType(BonusType bonusType) {
        _bonusType = bonusType;
        _sprRend.sprite = bonusImages[(int) bonusType-1];
    }
    
    private void ActivateBonus() {
        gameMgr.instance().PlayBonusSound();
        switch (_bonusType) {
            case BonusType.BON_BALLS:
                if (gameMgr.instance().GetNumberOfRays() == 1 && !gameMgr.instance().GetBall(0).attached) {
                    gameMgr.instance().GetBall(0).CreateADouble();
                }
                break;
            case BonusType.BON_SHRUNK:
                gameMgr.instance().GetGamePlatform().ShrinkPlatform();
                break;
            case BonusType.BON_ENLARGE:
                gameMgr.instance().GetGamePlatform().EnlargePlatform();
                break;
            case BonusType.BON_SPEEDUP:
                Ball.SpeedTheBallUp();
                break;
            case BonusType.BON_SLOWDOWN:
                Ball.SlowTheBallDown();
                break;
        }
    }

    private void FixedUpdate() {
        transform.Translate(-transform.up*Time.fixedDeltaTime*2f);
        if (_sprRend.bounds.Intersects(gameMgr.instance().GetGamePlatform().GetSpriteRenderer().bounds)) {
            ActivateBonus();
            Destroy(gameObject);
        }
    }
}
