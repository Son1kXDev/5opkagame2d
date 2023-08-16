using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairColor : MonoBehaviour
{
    [SerializeField, StatusIcon] private Material _hairMaterial;

    public void SetHairColor(Color hairColor)
    {
        _hairMaterial.SetColor("_HairColor", hairColor);
    }
}
