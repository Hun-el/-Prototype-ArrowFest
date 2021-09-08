using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : PoolObject
{
    int orbit;
    Vector3 direction;

    public void SetValues(int orbit)
    {
        this.orbit = orbit;
        direction = transform.localPosition;
    }

    public void SetLocation(float x, float y)
    {
        Vector3 targetPos = new Vector3(direction.x * x, direction.y * y, 0);
        transform.DOLocalMove(targetPos, 0.05f);
    }
}
