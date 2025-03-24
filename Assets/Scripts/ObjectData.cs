using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    [field:SerializeField] public int Id { get; private set; }
    [field:SerializeField] public bool IsNPC { get; private set; }
}
