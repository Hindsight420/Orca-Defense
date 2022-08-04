using EventCallbacks;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] GameObject counterPrefab;

    Dictionary<ResourceType, TextMeshProUGUI> resourceValueComponentMap;
    private SeasonManager seasonManager;

    private void Start()
    {
        seasonManager = SeasonManager.Instance;
        ResourceValueChangedEvent.RegisterListener(OnResourceValueChanged);
    }

    public void InitializeCounters(List<ResourceValue> resourceValues)
    {
        resourceValueComponentMap = new();

        foreach (ResourceType resourceType in DataSystem.Instance.ResourceTypes)
        {
            int amount = resourceValues.First(r => r.Type == resourceType).Amount;
            InitializeCounter(resourceType, amount);
        }
    }

    void InitializeCounter(ResourceType resourceType, int amount)
    {
        GameObject counterGO = Instantiate(counterPrefab, transform);

        counterGO.name = $"Counter - {resourceType.name}";
        counterGO.GetComponentsInChildren<Image>().First(i => i.name == "Image").sprite = resourceType.icon; // TODO: Remove string reference and convoluted get

        TextMeshProUGUI value = counterGO.GetComponentInChildren<TextMeshProUGUI>();
        value.text = amount.ToString();
        resourceValueComponentMap.Add(resourceType, value);
    }

    void OnResourceValueChanged(ResourceValueChangedEvent resourceValueChangedEvent)
    {
        ResourceValue resourceValue = resourceValueChangedEvent.ResourceValue;
        resourceValueComponentMap.TryGetValue(resourceValue.Type, out TextMeshProUGUI resourceValueComponent);
        resourceValueComponent.text = resourceValue.Amount.ToString();
    }
}
