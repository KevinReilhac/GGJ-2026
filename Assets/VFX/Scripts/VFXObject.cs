using UnityEngine;
using UnityEngine.VFX;

public class VFXObject : MonoBehaviour
{
    VisualEffect vfx;
    Transform destination;
    Vector3 startPos;
    float projSpeed;
    float timer;
    bool hasHit;
    Vector3 thirdPoint;

    public void SetVFX(VisualEffectAsset vfxGraph, Vector3 newStartPos, Transform newDestination, float newSpeed)
    {
        if(vfx == null)
            vfx = GetComponent<VisualEffect>();

        vfx.visualEffectAsset = vfxGraph;
        startPos = newStartPos;
        destination = newDestination;
        projSpeed = newSpeed;
        FindThirdPoint();

        vfx.gameObject.SetActive(true);
    }

    void FindThirdPoint()
    {
        thirdPoint = Vector3.Lerp(startPos, destination.position, 0.5f);
        
        thirdPoint += Random.insideUnitSphere * Vector3.Distance(startPos, destination.position) * 0.5f;
        thirdPoint += Vector3.up * 0.75f;
    }

    public void ThrowVFX()
    {
        hasHit = false;
        vfx.SendEvent("OnPlay");
        SFXPlayer._instance.MakeAttackSound(vfx.visualEffectAsset.name, true, startPos);

        timer = 0;
    }

    void Update()
    {
        if(!hasHit)
            TranslateVFX();
        else
            EndVFX();
    }

    void TranslateVFX()
    {
        timer += Time.deltaTime * projSpeed;
        transform.position = BezierCurve(timer);
        transform.LookAt(destination);

        if(timer >= 1 && !hasHit)
            HitVFX();
    }

    Vector3 BezierCurve(float currentTime)
    {
        float u = 1 - currentTime;
        float tt = currentTime * currentTime;
        float uu = u * u;

        Vector3 newPos = uu * startPos;
        newPos +=  2 * u * currentTime * thirdPoint;
        newPos += tt * destination.position;

        return newPos;
    }

    void HitVFX()
    {
        vfx.SendEvent("OnVFXHit");
        SFXPlayer._instance.MakeAttackSound(vfx.visualEffectAsset.name, false, destination.position);
        hasHit = true;
    }

    void EndVFX()
    {
        if(vfx.aliveParticleCount == 0)
            GetComponent<PoolObject>().ReturnToPool();
    }
}
