using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    public static bool isCloseToZero(Vector3 v) {
        return Mathf.Approximately(v.magnitude, 0.5f);
    }
}
