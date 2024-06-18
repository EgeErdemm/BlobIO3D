using System;
using UnityEngine;

public class Blob : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private int level = 1;
    [SerializeField] private float StartRadius = 0.15f;

    private const float RadiusPerLevel = 0.15f;

    public int Level => level;
    
    private void Update()
    {
        SetScale(level);
    }

    public void SetLevel(int level)
    {
        this.level = level;
        SetScale(level);
    }

    private void SetScale(int level)
    {
        float radius = (level - 1) * RadiusPerLevel + StartRadius;
        float scale = radius * 2;
        _pivot.localScale = scale * Vector3.one;
        _sphereCollider.radius = radius;
        Vector3 center = _sphereCollider.center;
        center.y = radius;
        _sphereCollider.center = center;
    }
    
}
