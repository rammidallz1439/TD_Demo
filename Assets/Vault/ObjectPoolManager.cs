using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vault
{
    public class ObjectPoolManager : IController
    {
        private static ObjectPoolManager instance;
        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        public static ObjectPoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPoolManager();
                }
                return instance;

            }
        }

        /// <summary>
        /// Creates a ool with given type of Object and capacity
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initialCapacity"></param>
        /// <param name="parent"></param>
        public void InitializePool(GameObject prefab, int initialCapacity, Transform parent = null)
        {
            string key = prefab.name;

            // Initialize a new pool only if it does not exist
            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<GameObject>();
                prefabDictionary[key] = prefab;

                for (int i = 0; i < initialCapacity; i++)
                {
                    GameObject newObj = MonoHelper.Instance.InstantiateObject(prefab, parent);
                    if (parent != null)
                        newObj.transform.position = parent.transform.position;
                    newObj.SetActive(false);
                    newObj.name = prefab.name;
                    poolDictionary[key].Enqueue(newObj);
                }
            }

        }

        /// <summary>
        /// Gets Object to pool
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="activeState"></param>
        /// <returns></returns>
        public GameObject Get(string prefabName, bool activeState, Transform parent = null)
        {
            string key = prefabName.Replace("(Clone)", "").Trim();
            if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
            {
                GameObject obj = poolDictionary[key].Dequeue();
                obj.SetActive(activeState);
                return obj;
            }
            else if (prefabDictionary.ContainsKey(key))
            {

                Debug.Log("Trying tyo Create Enemy");
                GameObject newObj = MonoHelper.Instance.InstantiateObject(prefabDictionary[key], parent);
                if (parent != null)
                    newObj.transform.position = parent.transform.position;
                newObj.SetActive(true);
                return newObj;

            }

            Debug.LogError("No pool exists for prefab: " + key);
            return null;
        }

        /// <summary>
        /// Returns Object to pool
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnToPool(GameObject obj)
        {
            string key = obj.name.Replace("(Clone)", "").Trim();

            if (poolDictionary.ContainsKey(key))
            {
                obj.SetActive(false);
                poolDictionary[key].Enqueue(obj);
            }
            else
            {
                MonoHelper.Instance.DestroyObject(obj);
            }
        }

        public void ClearPools()
        {
            prefabDictionary.Clear();
            poolDictionary.Clear();
        }


        #region contract 
        public void OnInitialized()
        {
        }

        public void OnRegisterListeners()
        {
        }

        public void OnRelease()
        {
        }

        public void OnRemoveListeners()
        {
        }

        public void OnStarted()
        {
        }

        public void OnVisible()
        {
        }
        #endregion
    }
}

