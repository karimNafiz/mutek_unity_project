
public class User<T> where T : User<T>
{
    public static T instance;
    public static T Instance => instance;

    public User() 
    {
        if (instance != null) return;
        instance = (T)this;
    }
    
}
