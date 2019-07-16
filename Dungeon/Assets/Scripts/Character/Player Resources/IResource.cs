using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResource
{
    bool UseResource(int amount);
    void AddResource(int amount);
}
