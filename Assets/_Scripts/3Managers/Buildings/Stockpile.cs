using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stockpile<T>: DataEntity
{
    protected int capacity;
    protected int currentQuantity;
    public int CurrentQuantity { get => currentQuantity; }
    public int Capacity { get => capacity; }

    protected Queue<T> ResourceQueue { get; set; }

    [SerializeField]
    private Transform NewResourceSpawnLocation;
    [SerializeField]
    private GameObject ResourcePrefab;
    private Logger _logger;

    protected Stockpile<T> ConfigureStore (int capacity)
    {
        this.capacity = capacity;
        ResourceQueue = new Queue<T>();
        _logger = Logger.Instance;

        return this;
    } 

    /// <summary>
    /// This will add a value to the Stockpile. Just adds one for now. Will probably need more work on it
    /// </summary>
    protected void AddToStore()
    {
        if (currentQuantity >= capacity)
        {
            return;
        }

        currentQuantity++;
        var go = Instantiate(ResourcePrefab, NewResourceSpawnLocation);
        var resource = go.GetComponent<T>();

        if (resource == null)
        {
            _logger.LogMessage("Couldn't find a resource when adding to stockpile", Logger.LogType.Error);
            return;
        }
        ResourceQueue.Enqueue(resource);

        var rigidbody = go.GetComponent<Rigidbody2D>();
        var randVector = new Vector2(Random.Range(-1,1), Random.Range(-1, 1));
        rigidbody.AddForce(randVector);
    }


}