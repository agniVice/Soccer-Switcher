using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour, ISubscriber
{
    [SerializeField] private Transform _ballPositionTop;
    [SerializeField] private Transform _ballPositionBottom;

    [SerializeField] private GameObject _particlePrefab;

    private Tween _scaleTween;
    private Tween _moveTween;

    private Vector2 _startScale;

    private bool _isMoving = false;
    private bool _isDies;

    private void Start()
    {
        GetComponentInChildren<TrailRenderer>().enabled = false;

        _ballPositionBottom.parent = null;
        _ballPositionTop.parent = null;

        _startScale = transform.localScale;
        transform.position = _ballPositionBottom.position;

        GetComponentInChildren<TrailRenderer>().enabled = true;
    }
    private void FixedUpdate()
    {
        if (!_isMoving)
        {
            if (!_isDies)
            {
                _scaleTween = transform.DOScale(1.4f, 4f).SetLink(gameObject).OnComplete(() => { DestroyBall(); });
                _isDies = true;
            }
        }
    }
    public void SubscribeAll()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
    }
    public void UnsubscribeAll()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();

        particle.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        particle.Play();

        Destroy(particle.gameObject, 2f);
    }
    private void OnPlayerMouseDown()
    {
        if (_isMoving)
            return;

        MoveTo(GetFartherTransform());
    }
    private Transform GetFartherTransform()
    {
        float distanceToTop = Vector3.Distance(transform.position, _ballPositionTop.position);
        float distanceToBottom = Vector3.Distance(transform.position, _ballPositionBottom.position);

        if (distanceToTop > distanceToBottom)
            return _ballPositionTop;
        else
            return _ballPositionBottom;
    }
    private void MoveTo(Transform transform)
    {
        if (_scaleTween != null)
            _scaleTween.Kill();

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Swap, 1f);

        _isDies = false;
        _isMoving = true;

        this.transform.DOScale(_startScale, 0.3f).SetLink(gameObject);
        _moveTween = this.transform.DOMove(transform.position, 0.6f).SetLink(gameObject).OnComplete(() =>
        { 
            _isMoving = false;

            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ScoreAdd, 1f);
            PlayerScore.Instance.AddScore();
        });
    }
    private void DestroyBall()
    {
        GameState.Instance.FinishGame();

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Burst, 1f);
        //AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);

        if (_moveTween != null)
            _moveTween.Kill();

        transform.DOScale(0, 0.1f).SetLink(gameObject);

        SpawnParticle();
        Destroy(gameObject, 0.2f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Foot"))
        {
            DestroyBall();
        }
    }
}
