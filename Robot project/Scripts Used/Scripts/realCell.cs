using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realCell : MonoBehaviour, Cell
{
    #region Variables

    [HideInInspector] public bool isMarked = false;
    [HideInInspector] public GameObject mark;
    [HideInInspector] public bool isOred = false;

    #endregion

    /// <summary>
    /// Marque cette case. Ne la marque pas si déjà marquée ou contenant le diamant
    /// 3D et traitements
    /// </summary>
    public bool Mark(GameObject tagAsset)
    {
        if (isMarked == false && isOred == false)
        {
            isMarked = true;
            mark = tagAsset;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    /// <summary>
    /// Supprime la marque existante (3d et traitements)
    /// </summary>
    public void DeleteMark()
    {
        if(isMarked == true)
        {
            isMarked = false;
            Destroy(mark);
        }     
    }
}
