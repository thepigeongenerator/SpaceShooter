namespace Source.Core;
internal abstract class Component {
    public GameObject gameObject;

    public T GetComponent<T>() where T : Component {
        return gameObject.GetComponent<T>();
    }
}
