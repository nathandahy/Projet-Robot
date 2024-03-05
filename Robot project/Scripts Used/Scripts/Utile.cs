using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utile
{
    #region Display

    public static void error()
    {
        error("error !");
    }

    public static void show(string message)
    {
        Debug.Log(message);
    }

    public static void error(string message)
    {
        Debug.LogError("<color=red> Personal </color> : " + message);
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public static void warning(string message)
    {
        Debug.LogWarning("<color=#ff8c00> Personal </color> : " + message);
    }

    public static void success(string message)
    {
        Debug.Log("<color=#00fa9a> Personal </color> : " + message);
    }

    public static void show<T>(T var)
    {
        Debug.Log(var.ToString());
    }

    public static void error<T>(T var)
    {
        Debug.LogError("<color=red> Personal </color> : " + var.ToString());
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public static void warning<T>(T var)
    {
        Debug.LogWarning("<color=#ff8c00> Personal </color> : " + var.ToString());
    }

    public static void success<T>(T var)
    {
        Debug.Log("<color=#00fa9a> Personal </color> : " + var.ToString());
    }

    public static void win()
    {
        success("success !");
    }

    #endregion


    #region Align

    public static void align(params GameObject[] GOToALign)
    {
        for (int i = 0; i < GOToALign.Length; i++)
        {
            align(GOToALign[i]);
        }
    }

    public static void align(params MonoBehaviour[] monosToALign)
    {
        for(int i = 0; i < monosToALign.Length;  i++)
        {
            align(monosToALign[i]);
        }
    }

    public static void align(params Transform[] transToALign)
    {
        for (int i = 0; i < transToALign.Length; i++)
        {
            align(transToALign[i]);
        }
    }

    public static void align(GameObject myGO)
    {
        align(myGO.GetComponent<Transform>());
    }


    public static void align(MonoBehaviour myMono)
    {
        align(myMono.GetComponent<Transform>());
    }

    public static void align(Transform myTrans)
    {
        myTrans.position = new Vector3(myTrans.position.x, myTrans.position.y);
    }

    #endregion


    #region Checks

    #region Not Null
    

    /// <summary>
    /// Verifie l'existence d'un nombre variable de paramètres
    /// </summary>
    /// <param name="objToCheck">un ou plusieurs objets à vérifier</param>
    public static void checkNotNull(params Object[] objToCheck)
    {
        for (int i = 0; i < objToCheck.Length; i++)
        {
            checkNotNull(objToCheck[i]);
        }
    }


    /// <summary>
    /// Verifie l'existence du paramètre
    /// </summary>
    /// <param name="myObject">objet à vérifier</param>
    public static void checkNotNull(Object myObject)
    {
        if (myObject == null)
        {
            error("An object is null");
        }
    }

    #endregion

    #region Not Zero

    /// <summary>
    /// Vérifie que le paramètre (flottant) n'est pas nul ou négatif
    /// </summary>
    /// <param name="var">un flottant à vérifier</param>
    public static void checkNotZeroOrLess(float var)
    {
        if(var <=  0)
        {
            error("An amount is egal or less than zero");
        }
    }

    /// <summary>
    /// Vérifie que que chaque paramètre (qui est un flottant) n'est pas nul ou négatif
    /// </summary>
    /// <param name="var">un ou plusieurs flottants à vérifier</param>
    public static void checkNotZeroOrLess(params float[] var)
    {
        for(int i = 0; i < var.Length; i ++)
        {
            checkNotZeroOrLess(var[i]);
        }
    }

    

    #endregion

    #region Must be higher

    /// <summary>
    /// Vérifie que le premier nombre indiqué est bien supérieur au deuxième.
    /// Méthode générique tant que les paramètres sont des comparables
    /// </summary>
    /// <typeparam name="T">T Typage générique qui doit être comparable</typeparam>
    /// <param name="high">valeur à tester de type générique T </param>
    /// <param name="low">valeur qui doit être inférieure de type générique T</param>
    public static void checkHigher<T>(T high, T low) where T : System.IComparable
    {
            if (low.CompareTo(high) >= 0)
            {
                 error("an amount should be higher than an other amount");
            }       
    }

    /// <summary>
    /// Vérifie que le paramètre indiqué est bien compris dans les bornes indiquées
    /// </summary>
    /// <typeparam name="T">T Typage générique qui doit être comparable</typeparam>
    /// <param name="value">valeur à tester de type générique T </param>
    /// <param name="low">borne inférieure de type générique T </param>
    /// <param name="high">borne supérieure de type générique T</param>
    public static void checkInBound<T>(T value, T low, T high) where T : System.IComparable
    {
        checkHigher<T>(value, low);
        checkHigher<T>(high, value);
    }

    #endregion

    #region Not Negative

    public static void checkNotNegative(int value)
    {
        checkHigher(value, -1);        
    }

    public static void checkNotNegative(params int[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            checkHigher(value[i], -1);
        }
    }

    #endregion

    #region Collections

    ///
    public static void checkDistinct<T>(T[] collection) where T : System.IEquatable<T>
    {
        for(int i = 0; i< collection.Length -1;i++)
        {
           for(int j = i+1; j <collection.Length;j++)
            { 
                if(collection[i].Equals(collection[j]))
                {
                    error("Elements of the collection not distinct");
                }
            }
        }
    }

    #endregion

    #endregion


    #region Others

    public static void freezeRotation(MonoBehaviour myMono)
    { 
        myMono.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX 
            | RigidbodyConstraints.FreezeRotationY 
            | RigidbodyConstraints.FreezePositionZ;
    }

    public static void assignColor(MonoBehaviour myMono, Color myColor)
    {
        myMono.gameObject.GetComponent<MeshRenderer>().material.color = myColor;
    }

    #endregion
}
