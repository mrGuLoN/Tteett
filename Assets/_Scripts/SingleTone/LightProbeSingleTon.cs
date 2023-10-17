using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProbeSingleTon : MonoBehaviour
{
    public static LightProbeSingleTon instance =null;
    public LightProbeGroup[] lightProbeGroups => _lightProbeGroups;
    [SerializeField] private LightProbeGroup[] _lightProbeGroups;
    void Awake()
    { 
        if (instance == null)
        {
            instance = this; 
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
