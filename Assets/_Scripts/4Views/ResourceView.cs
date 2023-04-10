using EventCallbacks;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField]
    private GameObject _counterPrefab;
    private Dictionary<ResourceType, TextMeshProUGUI> _resourceComponentMap;

    private void Start()
    {
        ResourcesChangedEvent.RegisterListener(OnResourceChanged);
    }

    public void InitializeCounters(ResourceList resources)
    {
        _resourceComponentMap = new();

        foreach (ResourceType resourceType in DataSystem.Instance.ResourceTypes)
        {
            int amount = resources.TryGetResource(resourceType).Amount;
            InitializeCounter(resourceType, amount);
        }
    }

    void InitializeCounter(ResourceType resourceType, int amount)
    {
        GameObject counterGO = Instantiate(_counterPrefab, transform);

        counterGO.name = $"Counter - {resourceType.name}";
        counterGO.GetComponentsInChildren<Image>().First(i => i.name == "Image").sprite = resourceType.Icon; // TODO: Remove string reference and convoluted get

        TextMeshProUGUI value = counterGO.GetComponentInChildren<TextMeshProUGUI>();
        value.text = amount.ToString();
        _resourceComponentMap.Add(resourceType, value);
    }

    void OnResourceChanged(ResourcesChangedEvent resourceChangedEvent)
    {
        foreach (var resourceElement in _resourceComponentMap)
        {
            Resource resource = resourceChangedEvent.ResourceList.TryGetResource(resourceElement.Key);
            resourceElement.Value.text = resource.Amount.ToString();
        }
    }
}
