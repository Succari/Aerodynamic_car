using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDistancePair
{
    public Particle particle;
    public float distance;

    public ParticleDistancePair(Particle particle, float distance)
    {
        this.particle = particle;
        this.distance = distance;
    }
}
