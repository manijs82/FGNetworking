using UnityEngine;
using UnityEngine.UI;


public class AmmoUI : MonoBehaviour
{
    [SerializeField] Text ammoCountText;
    [SerializeField] FiringAction firingAction;

    void Start()
    {
        firingAction.ammoCount.OnValueChanged += UpdateUI;
    }

    private void UpdateUI(int previousValue, int newValue)
    {
        ammoCountText.text = newValue.ToString();
    }
}