using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{


    private void OnTriggerExit2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Ground Unseen"))
            if (hitObject.transform.GetChild(0).gameObject.TryGetComponent(out FogSensor fadeSensor))
                fadeSensor.DoFadeOut();
    }

}
