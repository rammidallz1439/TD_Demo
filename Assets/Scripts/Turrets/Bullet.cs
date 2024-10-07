using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private BulletType _bulletType;
    public Transform Target = null;
    public float AttackPower;

    [Header("Rocket Data")]
    [SerializeField] private float _blastRadius;
    [SerializeField] private float _garvity;
    [SerializeField] private float _launchAngle;
    public Vector3 velocity;
    public GameObject BlastEffect = null;

    private void Start()
    {
        if (Target != null)
            EventManager.Instance.TriggerEvent(new CalcualteRocketVelocityEvent(_bulletType, this, _launchAngle, _garvity));
    }


    private void Update()
    {
        CheckBulletType(_bulletType);
    }


    private void OnTriggerEnter(Collider other)
    {
        CheckAttackType(_bulletType, other);
        if (other.gameObject.tag is "Enemy" || other.gameObject.tag is "Ground")
        {
            if (_bulletType != BulletType.Laser)
                Vault.ObjectPoolManager.Instance.ReturnToPool(gameObject);

        }

    }


    private void CheckBulletType(BulletType type)
    {
        switch (type)
        {
            case BulletType.None:
                break;
            case BulletType.Bullet:
                EventManager.Instance.TriggerEvent(new BulletEvent(Target, gameObject, _speed));
                break;
            case BulletType.Rocket:
                EventManager.Instance.TriggerEvent(new RocketEvent(Target, gameObject, _speed, _blastRadius, _garvity));
                break;
            case BulletType.Laser:
                break;
            default:
                break;
        }
    }

    private void CheckAttackType(BulletType type, Collider other)
    {
        switch (type)
        {
            case BulletType.None:
                break;
            case BulletType.Bullet:
                if (other.gameObject.tag is "Enemy")
                    other.transform.GetComponent<Enemy>().TakeDamage(AttackPower);
                break;
            case BulletType.Rocket:
                if (other.gameObject.tag is "Enemy" || other.gameObject.tag is "Ground")
                    EventManager.Instance.TriggerEvent(new RocketBlastEvent(AttackPower, gameObject, _blastRadius));
                break;
            case BulletType.Laser:
                if (other.gameObject.tag is "Enemy")
                {
                    if (other.transform != null)
                        other.transform.GetComponent<Enemy>().TakeDamage(AttackPower);

                    MEC.Timing.CallDelayed(0.3f, () =>
                    {
                        transform.position = transform.parent.transform.position;
                        gameObject.SetActive(false);
                    });

                }
                break;
            default:
                break;
        }
    }


}
