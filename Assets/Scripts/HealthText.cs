using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 125f, 0);

    public float elapsedTime = 0f;
    public float timeToFade = 1.0f;

    TextMeshProUGUI textMeshPro;
    RectTransform textTransform;

    private Color startColor;
    // Start is called before the first frame update
    void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    // Update is called once per frame
    void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (elapsedTime < timeToFade)
        {
            textMeshPro.alpha -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
