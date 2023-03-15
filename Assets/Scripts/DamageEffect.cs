using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float changeSpeed = 2f;
    [SerializeField] private float lifetime = 1f;

    private void Awake()
    {
        Destroy(gameObject, lifetime);

        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    private void FixedUpdate()
    {
        text.color = Color.Lerp(text.color, Color.clear, changeSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    public void SetDamage(int damage)
    {
        text.text = $"{damage}";
    }
}
