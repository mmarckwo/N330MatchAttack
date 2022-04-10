using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPersist : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
