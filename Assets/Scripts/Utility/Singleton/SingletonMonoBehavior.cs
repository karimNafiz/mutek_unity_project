using UnityEngine;
namespace Utility.Singleton
{
    public class SingletonMonoBehavior<T> : MonoBehaviour where T : SingletonMonoBehavior<T>
    {
        static T instance;
        public static T Instance => instance;
            

        protected virtual void Awake()
        {
            if (instance != null && instance != this) 
            {
                Destroy(this.gameObject);
                return;
            }
            instance = (T)this;

        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}