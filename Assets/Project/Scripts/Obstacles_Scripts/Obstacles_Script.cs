using UnityEngine;

public class Obstacles_Script : MonoBehaviour
{
    public int requiredRobots = 4;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int currentHordeSize = Horda_Manager.Instance.activeRobots.Count;

            if (currentHordeSize >= requiredRobots)
            {
                ExplodeObstacle();
            }
            else
            {
                Horda_Manager.Instance.RemoveRobot(collision.gameObject);
            }
        }
    }
    private void ExplodeObstacle()
    {
        Destroy(gameObject);
    }
}
