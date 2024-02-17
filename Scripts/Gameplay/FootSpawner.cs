using System;
using UnityEngine;

public class FootSpawner : MonoBehaviour
{
    public static FootSpawner Instance;

    [Header("Left")]
    [SerializeField] private Transform _leftPosition;
    [SerializeField] private Transform _leftTopPosition;
    [SerializeField] private Transform _leftBottomPosition;

    [Header("Right")]
    [SerializeField] private Transform _rightPosition;
    [SerializeField] private Transform _rightTopPosition;
    [SerializeField] private Transform _rightBottomPosition;

    [Header("Other")]
    [SerializeField] private GameObject _footPrefab;
    [SerializeField] private float _spawnRate;

    private float _currentTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
        if (_currentTime < _spawnRate)
        {
            _currentTime += Time.fixedDeltaTime;
        }
        else
        {
            _currentTime = 0f;
            _spawnRate = UnityEngine.Random.Range(0.2f, 2f);
            SpawnFoot();
        }
    }
    public void SpawnFoot()
    {
        bool spawnedLeft = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

        Vector2 spawnPositon = Vector2.zero;

        if (spawnedLeft)
            spawnPositon = new Vector2(_leftPosition.position.x, UnityEngine.Random.Range(_leftBottomPosition.position.y, _leftTopPosition.position.y));
        else
            spawnPositon = new Vector2(_rightPosition.position.x, UnityEngine.Random.Range(_rightBottomPosition.position.y, _rightTopPosition.position.y));

        var foot = Instantiate(_footPrefab, spawnPositon, Quaternion.identity);

        if (spawnedLeft)
            foot.transform.right = Vector2.right;
        else
            foot.transform.right = -Vector2.right;

        foot.GetComponent<Foot>().Initialize(5 + PlayerScore.Instance.Score / 10);
    }
}
