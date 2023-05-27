using UnityEngine;
using Photon.Pun;

public class RunnerMovement : MonoBehaviour
{
    public Transform Target { get; set; } // Made Target a property that can be set
    public float Speed { get; set;  }

    void Update()
    {
        if (Target != null)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);

            // If the object reached the target
            if (transform.position == Target.position)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
