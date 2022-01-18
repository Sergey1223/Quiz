using UnityEngine;
public static class ExtensionMethods
{
    public static GameObject Spawn(this Object @object, GameObject original, Vector2 position, Transform parent, Configuration configuration)
    {
        GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity, parent);

        if (gameObject.TryGetComponent(out IInitializable component))
        {
            component.Initialize(configuration);
        }

        return gameObject;
    }
}
