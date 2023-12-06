using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSliderController : MonoBehaviour
{
    [SerializeField]
    private Slider slider = null;
    private float currentStamina = 0.0f;

    public void GetStaminaValue(float _stamina)
    {
        currentStamina = _stamina;
        slider.value = currentStamina;
    }
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
