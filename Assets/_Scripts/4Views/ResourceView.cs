using EventCallbacks;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] GameObject counterPrefab;

    Dictionary<ResourceType, TextMeshProUGUI> resourceComponentMap;
    private SeasonManager seasonManager;

    private void Start()
    {
        seasonManager = SeasonManager.Instance;
        ResourcesChangedEvent.RegisterListener(OnResourceChanged);
    }

    public void InitializeCounters(ResourceList resources)
    {
        resourceComponentMap = new();

        foreach (ResourceType resourceType in DataSystem.Instance.ResourceTypes)
        {
            int amount = resources.TryGetResource(resourceType).Amount;
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
        resourceComponentMap.Add(resourceType, value);
    }

    void OnResourceChanged(ResourcesChangedEvent resourceChangedEvent)
    {
        foreach (var resourceElement in resourceComponentMap)
        {
            Resource resource = resourceChangedEvent.ResourceList.TryGetResource(resourceElement.Key);
            resourceElement.Value.text = resource.Amount.ToString();
        }
    }
}
