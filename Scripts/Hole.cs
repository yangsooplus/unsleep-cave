using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole: MonoBehaviour, CaveClickable
{
    public int holeIndex;

    void CaveClickable.onClick(CaveMapManager caveMapManager)
    {
        caveMapManager.proceed(holeIndex);
    }
}
