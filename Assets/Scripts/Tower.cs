using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)] private float _targetingRange = 1.5f;
    [SerializeField, Range(0.5f, 5f)] private float _shootingCooldown = 1f;
    [SerializeField, Range(1, 5)] private int _shootingDamage = 1;
    [SerializeField] private LineRenderer _shootingLineEffect;
    [SerializeField] private GameObject _targetingZone;

    private bool readyToShoot = true;
    
    private TargetPoint _target;
    private const int ENEMY_LAYER_MASK = 1 << 9;
    private const int WALL_LAYER_MASK = 1 << 10;
    private void Awake()
    {
        _targetingZone.SetActive(false);
        _targetingZone.transform.localScale = new Vector3(0.105f*2*_targetingRange, 0.105f*2*_targetingRange, 1f);
    }
    public override void GameUpdate()
    {
        if (isAcquireTarget() || isTargetTracked())
        {
            //Debug.Log("TargetFound");
            Shoot();
        }
    }

    public void OnMouseOver()
    {
        _targetingZone.SetActive(true);
    }
    public void OnMouseExit()
    {
        _targetingZone.SetActive(false);
    }

    private void Shoot()
    {
        if (readyToShoot && (Vector2.Distance(transform.position, _target.Position) < (_targetingRange + _target.ColliederSize)))
        {
            readyToShoot = false;
            StartCoroutine(ShootingCoolDownCoroutine());
            //Debug.Log("Shoot");
            //_target.Enemy.GetDamage(_shootingDamage);
            _shootingLineEffect.positionCount = 2;
            Vector3[] linePoses = new Vector3[2];
            linePoses[0] = transform.position;
            linePoses[1] = _target.Position;
            _shootingLineEffect.SetPositions(linePoses);
            StartCoroutine(ClearShootingEffect());
        }
        //var point = _target.Position;
        
    }

    private IEnumerator ShootingCoolDownCoroutine()
    {
        yield return new WaitForSeconds(_shootingCooldown);
        readyToShoot = true;
    }
    private IEnumerator ClearShootingEffect()
    {
        yield return new WaitForSeconds(0.15f);
        _shootingLineEffect.positionCount = 0;
    }

    private bool isAcquireTarget()
    {
        //Collider[] targets = Physics.OverlapSphere(transform.localPosition, _targetingRange,ENEMY_LAYER_MASK);
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _targetingRange, ENEMY_LAYER_MASK);
        if (targets.Length > 0)
        {
            foreach (var trgt in targets)
            {
                _target = trgt.GetComponent<TargetPoint>();
                if(!TargetBehindWall() || (Vector2.Distance(transform.position, trgt.transform.position) > _targetingRange + _target.ColliederSize))
                    break;
            }

            if(TargetBehindWall())
                return false;
            //_target = targets[0].GetComponent<TargetPoint>();
            return true;
        }

        _target = null;
        return false;
    }

    private bool TargetBehindWall()
    {
       // Collider2D[] targets = Physics2D.OverlapCircleAll(transform.localPosition, _targetingRange, ENEMY_LAYER_MASK);
       Vector2 direction = _target.Position-transform.position;
       RaycastHit2D[] walls = Physics2D.RaycastAll(transform.position, direction, direction.magnitude,WALL_LAYER_MASK);
       if (walls.Length > 0)
           return true;
       return false;
    }

    private bool isTargetTracked()
    {
        if (_target == null)
        {
            return false;
        }

        Vector2 myPos = transform.localPosition;
        Vector2 targetPos = _target.Position;
        if (Vector2.Distance(myPos, targetPos) > _targetingRange + _target.ColliederSize || TargetBehindWall())
        {
            _target = null;
            return false;
        }

        return true;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        //Gizmos.DrawSphere(position,_targetingRange);
        Gizmos.DrawWireSphere(position,_targetingRange);
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position,_target.Position);
        }
    }
}
