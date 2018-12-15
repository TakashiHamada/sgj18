using UnityEngine;


public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
//	[SerializeField] bool dontDestoryOnLoad = false;

	static T instance;

	public static T Instance
	{
		get
		{
            if( instance == null )
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

	//==================================================
	//==================================================
	protected virtual void Awake()
	{
		if( instance != null && instance != this )
        {
            Destroy( gameObject );
            return;
        }

        instance = (T)this;

		//if( dontDestoryOnLoad )
		{
			//DontDestroyOnLoad( this );
		}
    }
}
