using UnityEngine;

public class SimulationManager : Singleton<SimulationManager>
{
    #region Variables
    /// <summary>
    /// The ScriptableObject instance with the level data.
    /// </summary>
    [SerializeField] private LevelSO level;

    /// <summary>
    /// Parent GameObject for AWS resources.
    /// </summary>
    [SerializeField] private GameObject resourceCollection;
    #endregion

    #region Properties
    public LevelSO Level
    {
        get { return this.level; }
    }
    #endregion

    #region Simulation Start
    void Start()
    {
        // Populate ResourceCollection with parent objects.
        // Get the services for the level.
        foreach (ServiceSO service in this.level.AWSServices)
        {
            GameObject serviceGO = new GameObject();
            serviceGO.name = service.ID;
            serviceGO.transform.parent = this.resourceCollection.transform;

            foreach (ResourceSO resource in service.ResourceInstances)
            {
                GameObject resourceGO = new GameObject();
                resourceGO.name = resource.ID;
                resourceGO.transform.parent = serviceGO.transform;
            }
        }
    }
    #endregion
}
