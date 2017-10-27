using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject obj = new GameObject();
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			//If I am the first instance, make me the Singleton
			instance = this as T;
			//DontDestroyOnLoad(this);
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it
			if (this != instance)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void Destroy()
	{
		if (instance != null)
		{
			Destroy(this.gameObject);
		}
	}
}
