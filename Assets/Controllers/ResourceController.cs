using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    int dogecoin;
    public int Dogecoin { get => dogecoin; private set => dogecoin = value; }

    float time = 0.0f;
    public float gainzz = 0.5f;

    public GameObject Crypto;

    public static ResourceController Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= gainzz)
        {
            time = 0.0f;
            dogecoin++;
        }

        Crypto.GetComponent<TextMeshProUGUI>().text = $"{dogecoin} Ð";
    }
}
