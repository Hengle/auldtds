using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAIv2 : MonoBehaviour
{
    #region Variables
    [Header("Targets")]
        [SerializeField]
        private GameObject currentTarget;
        [SerializeField]
        private List<GameObject> possibleTargetsList;
        [SerializeField]
        private float possibleTargetDistance;
        [SerializeField]
        private string[] targetTags;
    [SerializeField]
    private GameObject wpObject;

    private NavMeshAgent navigationAgent;
    #endregion

    #region System Functions
    // Use this for initialization
    void Start ()
    {
        navigationAgent = this.GetComponent<NavMeshAgent>();
        var path = new NavMeshPath();
        Vector3 destination = currentTarget.transform.position;
        navigationAgent.CalculatePath(destination, path);
        Debug.Log("Number of Waypoints " + path.corners.Length);
        for (int i = 0; i < path.corners.Length; i++)
        {
            Debug.Log(path.corners[i]);
            GameObject wpoint = (GameObject)Instantiate(wpObject, path.corners[i], this.transform.rotation);
        }
        navigationAgent.SetDestination(currentTarget.transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        AddPossibleTargetList();
        ClearMissingObjects();
        SortTargetsByDistance();
        //currentTarget = possibleTargetsList[0];
        
        
       // Debug.Log(navigationAgent.pathStatus);

    }
    #endregion

    #region General Functions
    private void AddPossibleTargetList()
    {
        foreach (string tag in targetTags)
        {
            GameObject[] templist = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject item in templist)
            {
                if (CheckDistanceToTarget(item))
                {
                    if (!possibleTargetsList.Contains(item))
                    {
                        possibleTargetsList.Add(item);
                    }
                }
                else
                {
                    if (possibleTargetsList.Contains(item))
                    {
                        possibleTargetsList.Remove(item);
                    }
                }
            }
        }
    }

    private bool CheckDistanceToTarget(GameObject targetObject)
    {
        float distanceToTarget = Vector3.Distance(this.gameObject.transform.position, targetObject.gameObject.transform.position);
        if (distanceToTarget <= possibleTargetDistance)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    private void ClearMissingObjects()
    {
        possibleTargetsList.RemoveAll(GameObject => GameObject == null);
    }

    private void SortTargetsByDistance()
    {
        possibleTargetsList.Sort(delegate (GameObject c1, GameObject c2)
        {
            return Vector3.Distance(this.transform.position, c1.transform.position).CompareTo
               ((Vector3.Distance(this.transform.position, c2.transform.position)));
        });
    }
    #endregion
}
