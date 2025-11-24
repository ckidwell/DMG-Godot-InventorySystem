using System.Collections;
using System.Collections.Generic;

namespace DMGInventorySystem.Utils
{
    public class GameObjectUtils //: MonoBehaviour
    {
// maybe don't need this depending on Godot stuff
        // public static T FindInParents<T>(GameObject go) where T : Component
        // {
        //     if (go == null) return null;
        //     var comp = go.GetComponent<T>();
        //
        //     if (comp != null)
        //         return comp;
        //
        //     Transform t = go.transform.parent;
        //     while (t != null && comp == null)
        //     {
        //         comp = t.gameObject.GetComponent<T>();
        //         t = t.parent;
        //     }
        //
        //     return comp;
        // }
        //
        // public static void DestroyChildByName(GameObject go, string _name)
        // {
        //     if (go != null)
        //     {
        //         Transform goTransform = go.transform;
        //         foreach (Transform child in goTransform)
        //         {
        //             if (child.name == _name)
        //             {
        //                 GameObject.Destroy(child.gameObject);
        //             }
        //
        //             foreach (Transform childrenToo in child)
        //             {
        //                 if (childrenToo.name == _name)
        //                 {
        //                     GameObject.Destroy(childrenToo.gameObject);
        //                 }
        //             }
        //         }
        //     }
        //
        // }
        //
        // public static void DestroyChildren(GameObject go)
        // {
        //
        //     if (go != null)
        //     {
        //         Transform goTransform = go.transform;
        //         foreach (Transform child in goTransform)
        //         {
        //             GameObject.Destroy(child.gameObject);
        //         }
        //     }
        //
        // }
        //
        // public static void DestroyChildrenImmediately(GameObject go)
        // {
        //
        //     if (go != null)
        //     {
        //         Transform goTransform = go.transform;
        //         foreach (Transform child in goTransform)
        //         {
        //             GameObject.DestroyImmediate(child.gameObject);
        //         }
        //     }
        //
        // }
    }

}