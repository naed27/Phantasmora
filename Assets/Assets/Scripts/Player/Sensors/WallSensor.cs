
using System.Collections;
using UnityEngine;

public class WallSensor : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collisionTarget)
    {
        //GameObject hitObject = collisionTarget.gameObject;
        
        //if (hitObject.TryGetComponent(out FogSensor fadeSensor))
        //{
        //    if(hitObject.layer == LayerMask.NameToLayer("Wall Unseen"))
        //        fadeSensor.DoFadeIn();
         //   if (hitObject.layer == LayerMask.NameToLayer("Ground Unseen"))
        //        return;
        //}

        //if (collisionTarget.gameObject.layer == LayerMask.NameToLayer("Wall Unseen"))
           // collisionTarget.gameObject.layer = LayerMask.NameToLayer("Wall Seen");
    }

    private void OnTriggerExit2D(Collider2D collisionTarget)
    {
        //GameObject hitObject = collisionTarget.gameObject;
        //if (hitObject.layer == LayerMask.NameToLayer("Wall Unseen"))
          //  if (hitObject.transform.GetChild(0).gameObject.TryGetComponent(out FogSensor fadeSensor))
                //fadeSensor.DoFadeOut();
    }

}