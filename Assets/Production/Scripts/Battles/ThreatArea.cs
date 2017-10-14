using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThreatArea : MonoBehaviour
{
    [SerializeField]
    private float scriptVersion = 1.2f;
    [SerializeField]
    private GameObject parentObject;
    [SerializeField]
    private LayerMask rayCastLayer;
    [SerializeField]
    private string enemyTag;
    //private GameObject threatManager;
    //private ThreatManager threatManagerScript;

    public List<GameObject> enemiesListOnSight = new List<GameObject>();
    public List<GameObject> enemiesListOutOfSight = new List<GameObject>();
    private List<GameObject> temporaryEnemiesList = new List<GameObject>();

    private float enemyDistance;

    [SerializeField]
    private GameObject currentTarget;
    [SerializeField]
    private GameObject noTargetObject;

    [SerializeField]
    private bool targetLock;


    // Use this for initialization
    void Start()
    {
        //threatManagerScript = threatManager.GetComponent<ThreatManager>();
        parentObject.GetComponent<Animator>().enabled = false;	
    }

    // Update is called once per frame
    void Update()
    {
        SortEnemiesByDistance();
        CheckLineOfSight();
        ChooseClosestTarger();
        //VisualizeLineOfSight();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == enemyTag)
        {
            enemyDistance = Vector3.Distance(this.gameObject.transform.position, other.gameObject.transform.position);
            RaycastHit hit;
            if (Physics.Linecast(this.gameObject.transform.position, other.gameObject.transform.position, out hit, rayCastLayer))
            {
                enemiesListOutOfSight.Add(other.gameObject);
            }
            else
            {
                enemiesListOnSight.Add(other.gameObject);
                SortEnemiesByDistance();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == enemyTag)
        {
            if (enemiesListOnSight.Contains(other.gameObject))
            {
                enemiesListOnSight.Remove(other.gameObject);
            }
            else if (enemiesListOutOfSight.Contains(other.gameObject))
            {
                enemiesListOutOfSight.Remove(other.gameObject);
            }
        }
        currentTarget = noTargetObject;
        targetLock = false;
    }

    private void SortEnemiesByDistance()
    {
        enemiesListOnSight.RemoveAll(GameObject => GameObject == null);
        enemiesListOutOfSight.RemoveAll(GameObject => GameObject == null);
        enemiesListOnSight.Sort(delegate (GameObject c1, GameObject c2)
        {
            return Vector3.Distance(this.transform.position, c1.transform.position).CompareTo
               ((Vector3.Distance(this.transform.position, c2.transform.position)));
        });

        enemiesListOutOfSight.Sort(delegate (GameObject c1, GameObject c2)
        {
            return Vector3.Distance(this.transform.position, c1.transform.position).CompareTo
               ((Vector3.Distance(this.transform.position, c2.transform.position)));
        });
    }
    
    
    private void CheckLineOfSight()
    {
        temporaryEnemiesList = enemiesListOnSight.Concat(enemiesListOutOfSight).ToList();
        foreach (GameObject enemyObject in temporaryEnemiesList)
        {
            RaycastHit hit;
            if (Physics.Linecast(this.gameObject.transform.position, enemyObject.transform.position, out hit, rayCastLayer))
            {
                if (enemiesListOnSight.Contains(enemyObject))
                {
                    enemiesListOnSight.Remove(enemyObject);
                    if (!enemiesListOutOfSight.Contains(enemyObject))
                    {
                        enemiesListOutOfSight.Add(enemyObject);
                    }
                }             
            }
            else if (!Physics.Linecast(this.gameObject.transform.position, enemyObject.transform.position, out hit, rayCastLayer))
            {
                if (enemiesListOutOfSight.Contains(enemyObject))
                {
                    enemiesListOutOfSight.Remove(enemyObject);
                    if (!enemiesListOnSight.Contains(enemyObject))
                    {
                        enemiesListOnSight.Add(enemyObject);
                    }
                }
            }
        }
        ClearTargetWhenOutOfSight();
    }
    /******************************************************************************************************************/
    private void FaceEnemy()
    {
        Vector3 relativePos = currentTarget.transform.position - parentObject.transform.position;
        Quaternion unitRotation = Quaternion.LookRotation(relativePos);
        parentObject.transform.rotation = unitRotation;
    }
    private void ChooseClosestTarger()
    {
        if (!targetLock)
        {
            if (enemiesListOnSight.Count >0)
            {
                //Debug.Log("Getting Closest Target");
                currentTarget = enemiesListOnSight[0];
                targetLock = true;
                AttackCurrentTarget();
            }
        }
        if (currentTarget == null)
        {
            targetLock = false;
        }
        
    }

    private void AttackCurrentTarget()
    {
        parentObject.GetComponent<Animator>().enabled = true;
        FaceEnemy();
    }

    private void ClearTargetWhenOutOfSight()
    {
        if (enemiesListOutOfSight.Contains(currentTarget))
        {
            currentTarget = noTargetObject;
            targetLock = false;
        }
    }
}