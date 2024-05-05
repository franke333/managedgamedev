using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public int Health { get; }

    // Returns true if the object is destroyed
    public bool TakeDamage(int damage);
}
