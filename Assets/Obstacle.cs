using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Obstacle : MonoBehaviour
{
    [FormerlySerializedAs("sprRend")] [SerializeField]
    protected SpriteRenderer _sprRend;
    protected List<bool> _justHit;
    protected List<float> _coolTimer;
    protected const float KCoolTime = 0.10f;
    protected int _health = 0;
    protected List<Vector3> _frozenDirection;

    private void Awake() {
        _justHit = new List<bool>();
        _coolTimer = new List<float>();
        _frozenDirection = new List<Vector3>();
        _justHit.Add(false);
        _coolTimer.Add(10);
        _frozenDirection.Add(Vector3.zero);
    }

    protected bool HitCheck() {
        for (int i = 0; i < gameMgr.instance().GetNumberOfRays(); i++) {

            if (gameMgr.instance().GetBall(i)==null) {
                print(i);
                _justHit.RemoveAt(i);
                _frozenDirection.RemoveAt(i);
                _coolTimer.RemoveAt(i);
            }
            
            if (_justHit.Count <= i) {
                _justHit.Add(false);
                _coolTimer.Add(10);
                _frozenDirection.Add(Vector3.zero);
            }
            if(_justHit[i]) {
                if (_coolTimer[i] < 0) {
                    _justHit[i] = false;
                    _coolTimer[i] = 10;
                } else {
                    _coolTimer[i]-= Time.deltaTime;
                }
            } 
            Ray ray = gameMgr.instance().GetRay(i);
            if (_sprRend.bounds.Intersects(gameMgr.instance().GetBall(i).GetSpriteRenderer().bounds)) {
                if (!_justHit[i]) {

                    if (_justHit.Count >= i) {
                        _justHit[i] = true;
                        _coolTimer[i] = KCoolTime;
                    } 
                    Vector3 closestPoint = _sprRend.bounds.ClosestPoint(ray.origin);
                    float angle = Vector3.Angle(ray.origin - closestPoint, transform.up);
                    float angle2 = Vector3.Angle(ray.origin - closestPoint, transform.right);
                    float angle3 = Vector3.Angle(ray.origin - closestPoint, transform.forward);
                    Vector3 directionVector = Vector3.zero;
                    if (angle - angle2 < 0) {
                        if (angle == 0) {
                            directionVector = transform.up;
                        } else if (angle == 90){
                            directionVector = -transform.right;
                        }else {
                            directionVector = -transform.right;
                        }
                    } else if (angle - angle2 > 0) {
                        if (angle2 == 90) {
                            directionVector = -transform.up;
                        } else if (angle2 == 0){
                            directionVector = transform.right;
                        }else {
                            directionVector = -transform.up;
                        }
                    } else {
                        directionVector = -transform.up;
                        _coolTimer[i] *= 2;
                    }
                    
                    float angle1 = Vector3.Angle(ray.direction, directionVector);
                    float sign = angle1 > 90 ? 1 : -1;
                    Vector3 direction = Vector3.Reflect(ray.direction, sign * directionVector);
                    if (_frozenDirection.Count > i) {
                        _frozenDirection[i] = direction;
                    }
                    gameMgr.instance().BallHit(direction,i);
                    return true;
                } else {
                    gameMgr.instance().BallHit(_frozenDirection[i],i);
                }
            }
            
            
        }
        
        return false;
    }
}
