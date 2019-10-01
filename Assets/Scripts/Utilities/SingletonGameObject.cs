using UnityEngine;

public class SingletonGameObject<T> : MonoBehaviour where T : MonoBehaviour {

    private static T _instance;
    
    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();
           // DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            if (_instance != GetComponent<T>())
            {
                Destroy(gameObject);
            }
        }
    }
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new GameObject("Singleton For " + typeof(T).ToString());

                Debug.LogError("Creating Singleton For " + typeof(T).ToString());

               // DontDestroyOnLoad(singleton);
                _instance = singleton.AddComponent<T>();
                return _instance;
            }

            else return _instance;
        }
    } 
}
