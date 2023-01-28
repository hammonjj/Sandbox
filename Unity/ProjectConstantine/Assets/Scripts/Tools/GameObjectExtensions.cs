using System.Collections.Generic;
using UnityEngine;

public class GameObjectExtensions
{
    //Gets a tagged object in a scene regardless of whether it is active or not
    public static GameObject FindGameObjectWithTag(string tag)
    {
        var gameObjects = FindAllObjectsInScene();
        foreach(var gameObject in gameObjects)
        {
            if(gameObject.tag != tag)
            {
                continue;
            }

            return gameObject;
        }

        return null;
    }

    //Gets a tagged object in a scene regardless of whether it is active or not
    public static List<GameObject> FindGameObjectsWithTags(List<string> tags)
    {
        var ret = new List<GameObject>();
        var gameObjects = FindAllObjectsInScene();
        foreach(var gameObject in gameObjects)
        {
            if(!tags.Contains(gameObject.tag))
            {
                continue;
            }

            ret.Add(gameObject);
        }

        return ret;
    }

    //Gets all tagged objects in a scene regardless of whether they are active or not
    public static List<GameObject> FindGameObjectsWithTag(string tag)
    {
        var ret = new List<GameObject>();
        var gameObjects = FindAllObjectsInScene();
        foreach(var gameObject in gameObjects)
        {
            if(gameObject.tag != tag)
            {
                continue;
            }

            ret.Add(gameObject);
        }

        return ret;
    }

    
    public static List<GameObject> FindAllObjectsInScene()
    {
        UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        List<GameObject> objectsInScene = new List<GameObject>();

        for(int i = 0; i < rootObjects.Length; i++)
        {
            objectsInScene.Add(rootObjects[i]);
        }

        for(int i = 0; i < allObjects.Length; i++)
        {
            if(allObjects[i].transform.root)
            {
                for(int i2 = 0; i2 < rootObjects.Length; i2++)
                {
                    if(allObjects[i].transform.root == rootObjects[i2].transform && allObjects[i] != rootObjects[i2])
                    {
                        objectsInScene.Add(allObjects[i]);
                        break;
                    }
                }
            }
        }

        return objectsInScene;
    }

    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach(Transform child in parent)
        {
            if(child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if(found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
}
