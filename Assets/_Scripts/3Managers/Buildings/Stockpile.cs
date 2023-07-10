using System.Collections.Generic;
using UnityEngine;

public abstract class Stockpile<T> : DataEntity
{
    [SerializeField]
    private Transform _newResourceSpawnLocation;
    [SerializeField]
    private GameObject _resourcePrefab;

    private Logger _logger;
    private Queue<T> _resourceQueue;

    private int _capacity;
    private int _currentQuantity;
    public int CurrentQuantity { get => _currentQuantity; }
    public int Capacity { get => _capacity; }

    protected Stockpile<T> ConfigureStore(int capacity)
    {
        _capacity = capacity;
        _resourceQueue = new Queue<T>();
        _logger = Logger.Instance;

        return this;
    }

    /// <summary>
    /// This will add a value to the Stockpile. Just adds one for now. Will probably need more work on it
    /// </summary>
    protected void AddToStore()
    {
        if (_currentQuantity >= _capacity)
        {
            return;
        }

        _currentQuantity++;
        var go = Instantiate(_resourcePrefab, _newResourceSpawnLocation);
        var resource = go.GetComponent<T>();

        if (resource == null)
        {
            _logger.LogMessage("Couldn't find a resource when adding to stockpile", Logger.LogType.Error);
            return;
        }
        _resourceQueue.Enqueue(resource);

        var rigidbody = go.GetComponent<Rigidbody2D>();
        var randVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        rigidbody.AddForce(randVector);
    }
}
