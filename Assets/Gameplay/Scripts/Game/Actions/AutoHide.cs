using UnityEngine;

public class AutoHide : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
        Destroy(this);
    }
}
