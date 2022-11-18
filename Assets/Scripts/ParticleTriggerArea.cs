using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleTriggerArea : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    AreaEffector2D effector;
    Vector3 effectorForce;
    

    void OnValidate()
    {
        ps = GetComponent<ParticleSystem>();
        effector = ps.trigger.GetCollider(0).gameObject.GetComponent<AreaEffector2D>();
        effectorForce = DegreeToVector2(effector.forceAngle) * effector.forceMagnitude;   
    }

    private void OnParticleTrigger()
    {
        // ToDO: dont use the inside trigger, try to get the enter work instead.
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle p = inside[i];
            //p.startColor = new Color32(255, 0, 0, 255);
            //float r = Random.Range(0f, effector.forceVariation);
            p.velocity = new Vector3(effectorForce.x, effectorForce.y, 0f);
            inside[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
