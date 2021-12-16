/*
===================================================================
Unity Assets by MAKAKA GAMES: https://makaka.org/o/all-unity-assets
===================================================================

Online Docs (Latest): https://makaka.org/unity-assets
Offline Docs: You have a PDF file in the package folder.

=======
SUPPORT
=======

First of all, read the docs. If it didn’t help, get the support.

Web: https://makaka.org/support
Email: info@makaka.org

If you find a bug or you can’t use the asset as you need, 
please first send email to info@makaka.org
before leaving a review to the asset store.

I am here to help you and to improve my products for the best.
*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[HelpURL("https://makaka.org/unity-assets")]
[AddComponentMenu ("Makaka Games/Everyday Tools/Random Object Pooler")]
public class RandomObjectPooler : MonoBehaviour
{
    [Range(1, 30)]
    public int initPooledAmount = 7;
	public Transform poolParent = null;

    [Space]
    [SerializeField]
    private bool isDebugLogging = false;

    [Space]
    public Transform positionAtInit = null;
    public Transform rotationAtInit = null;

    [Header("Single (actual for Testing target prefab; None => Multiple)")]
    public GameObject prefab;

    [Header("Multiple")]
    public bool areRandomizedObjects = false;
	public GameObject[] prefabs;

	[Header("Events")]
    [Space]
    public UnityEvent OnInitialized;

	[HideInInspector]
	public List<GameObject> pooledObjects = null;

	private GameObject currentInstantiated = null;

    [HideInInspector]
    public List<MonoBehaviour> controlScripts;
	
    private MonoBehaviour controlScriptTempForRegistration;
    
    private System.Type controlScriptType;
	
	private void Start()
    {
        InitAndPopulatePool();
    }

    private void InitAndPopulatePool()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < initPooledAmount; i++)
        {
            pooledObjects.Add(InstantiateObject(i));
        }

        OnInitialized?.Invoke();
    }

    public void InitControlScripts(System.Type type)
    {
        controlScripts = new List<MonoBehaviour>();

        controlScriptType = type;
    }

    private GameObject InstantiateObject(int index)
    {
        GameObject tempPrefab;

        if (prefab)
        {
            tempPrefab = prefab;
        }
        else if (areRandomizedObjects)
        {
            tempPrefab = prefabs[Random.Range(0, prefabs.Length - 1)];
        }
        else
        {
            tempPrefab = prefabs[index % prefabs.Length];
        }

        currentInstantiated = Instantiate(
                tempPrefab,
                positionAtInit
                    ? positionAtInit.position
                    : tempPrefab.transform.position,
                rotationAtInit
                    ? rotationAtInit.rotation
                    : tempPrefab.transform.rotation,
                poolParent);

        currentInstantiated.SetActive(false);
        currentInstantiated.name = tempPrefab.name + index;

        return currentInstantiated;
    }

    public GameObject GetPooledObject()
	{
        for (int i = 0; i < pooledObjects.Count; i++) 
		{
			if (!pooledObjects[i])
            {
                //print("GetPooledObject(): Create New Instance");
                
                pooledObjects[i] = InstantiateObject(i);

                return pooledObjects[i];
            }

            if (!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}    
		}

        if (isDebugLogging)
        {
            DebugPrinter.Print("GetPooledObject():" +
                " All Game Objects in Pool are not available");
        }

        return null;
	}

    /// <summary>
    /// For initial registration (cashing)
    /// and subsequent getting Control Script of GameObject
    /// </summary>
    public MonoBehaviour RegisterControlScript(GameObject gameObject)
    {
		controlScriptTempForRegistration = null;

		// Search of cached Control Script
		for (int i = 0; i < controlScripts.Count; i++)
		{
			controlScriptTempForRegistration = controlScripts[i];

			if (controlScriptTempForRegistration)
			{
				if (controlScriptTempForRegistration.gameObject == gameObject)
				{	
					//print(i);
					
					break;
				}
				else
				{
					controlScriptTempForRegistration = null;
				}
			}
			else // Game Object is null
			{
				controlScripts.RemoveAt(i);

				//print("Remove null Control Script from List");
			}
		}

		if (!controlScriptTempForRegistration)
		{
			controlScriptTempForRegistration =
                gameObject.GetComponent(controlScriptType) as MonoBehaviour; 

            //print("Try to get Control Script");

            if (controlScriptTempForRegistration)
            {
			    controlScripts.Add(controlScriptTempForRegistration);
			    //print("Register New Control Script");
            }

		}

		return controlScriptTempForRegistration;
    }
}