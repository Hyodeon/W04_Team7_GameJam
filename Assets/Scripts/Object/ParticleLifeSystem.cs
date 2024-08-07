using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeSystem : MonoBehaviour
{
    [SerializeField] private float _lifeTime;


    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
