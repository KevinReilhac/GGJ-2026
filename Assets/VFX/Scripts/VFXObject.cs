using UnityEngine;
using UnityEngine.VFX;

public class VFXObject : MonoBehaviour
{
    VisualEffect vfx;
    Transform destination;
    Vector3 startPos;
    Vector3 goPos;
    Vector3 toPos;
    float projSpeed;
    float timer;
    bool hasHit;
    Vector3 thirdPoint;
    bool needBounce;
    bool needMove;

    public void SetVFX(VisualEffectAsset vfxGraph, Vector3 newStartPos, Transform newDestination, float newSpeed, bool moving = true)
    {
        if(vfx == null)
            vfx = GetComponent<VisualEffect>();

        vfx.visualEffectAsset = vfxGraph;

        startPos = newStartPos;
        destination = newDestination;

        goPos = startPos;
        toPos = destination.position;

        projSpeed = newSpeed;
        needMove = moving;
        
        FindThirdPoint();

        vfx.gameObject.SetActive(true);
    }

    void FindThirdPoint()
    {
        thirdPoint = Vector3.Lerp(goPos, toPos, 0.5f);
        
        thirdPoint += Random.insideUnitSphere * Vector3.Distance(goPos,toPos) * 0.5f;
        thirdPoint += Vector3.up * 0.5f;
    }

    public void ThrowVFX(bool bounce)
    {
        needBounce = bounce;

        hasHit = false;
        vfx.SendEvent("OnPlay");
        SFXPlayer._instance.MakeAttackSound(vfx.visualEffectAsset.name, true, goPos);

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

        if(needMove)
        {
            transform.LookAt(destination);
        }

        if(timer >= 1 && !hasHit)
            HitVFX();
    }

    Vector3 BezierCurve(float currentTime)
    {
        float u = 1 - currentTime;
        float tt = currentTime * currentTime;
        float uu = u * u;

        Vector3 newPos = uu * goPos;
        newPos +=  2 * u * currentTime * thirdPoint;
        newPos += tt * toPos;

        return newPos;
    }

    void HitVFX()
    {
        if(needBounce)
        {
            Bounce();
        }
        else
        {
            vfx.SendEvent("OnVFXHit");
            SFXPlayer._instance.MakeAttackSound(vfx.visualEffectAsset.name, false, destination.position);
            hasHit = true;       
        }
    }

    void EndVFX()
    {
        ResetVFX();
        if(vfx.aliveParticleCount == 0)
            GetComponent<PoolObject>().ReturnToPool();
    }

    void ResetVFX()
    {
        timer = 0;
        needMove = true;

        toPos = destination.position;
        goPos = startPos;
    }

    void Bounce()
    {
        toPos = startPos;
        goPos = destination.position;

        VFXManager._instance.UseShield();
        needBounce = false;
        ThrowVFX(false);
    }
}
