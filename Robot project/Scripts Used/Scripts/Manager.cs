using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui servira de point d'entrée unique pour les champs sérialisés.
/// Répartit les données entre Planet et Robot
/// </summary>
public class Manager : MonoBehaviour
{

    #region Variables

    #region Serialize

    #region References


    /// <summary>
    /// Regroupement des champs sérialisés liés à des références sur des objets.
    /// Tous ces objets sont nécessaires au déroulement du programme
    /// </summary>
    [Header("References/3D Assets")]
    [SerializeField] realCell cellAsset;  // CellAsset, robotAsset et thePlanet désigne un script/instance de classe,
    [SerializeField] realRobot robotAsset; // mais ce script doit être attaché à l'objet 3d voulu pour le représenter
    [SerializeField] realPlanet thePlanet; // il faut donc un objet ayant un composant script.
    [SerializeField] GameObject diamondAsset;
    [SerializeField] GameObject tagAsset;
    [SerializeField] Transform initialPosMap;
    [SerializeField] Camera refCamera;
    [SerializeField][Tooltip("Peut contenir un nombre aussi grand que voulu d'éléments 3D")] 
    GameObject[] pitonElements; 



    #endregion


    #region Planet


    /// <summary>
    /// Regroupe tous les champs sérialisés qui seront transmis à Planet
    /// </summary>
    [Header("Planet")]
    [SerializeField] Vector2Int minDimensionMap;  //Pour des dimensions aléatoires, on personnalisera ces bornes
    [SerializeField] Vector2Int maxDimensionMap; // Pour des dimensions fixes on met les mêmes valeurs dans ces deux champs
    [SerializeField] Vector2Int initialPositionRobot;
    [SerializeField] bool isRandomInitialPosXRobot; // On peut choisir une colonne fixe et une ligne aléatoire
    [SerializeField] bool isRandomInitialPosYRobot; // D'où la séparation en deux champs distincts
    [SerializeField] Vector2Int positionDiamond;
    [SerializeField] bool randomPosDiamond; // La position aléatoire du diamant n'est elle pas divisée en deux champs. 
                                            // Colonne et ligne sont fixes ou sont aléatoires les deux.
    [SerializeField] Direction orientationRobot; 
    [SerializeField] bool randomOrientationRobot; // si coché on ne prendra pas en compte orientationRobot
    [SerializeField] bool isInCorner;
    [SerializeField] Corner cornerRobot; // si isInCorner est coché, on place le robot dans le coin indiqué. Sinon n'est pas activé
    [SerializeField] Material materialOriginalCell;
    [SerializeField] Material materialGrid;

    #endregion


    #region Adjustment

    /// <summary>
    /// Regroupe tous les champs sérialisés liés à des ajustements.
    /// On trouve surtout des réglages liés à la hauteur des modèles 3D par rapport 
    /// à la planet (map). Des changements seront notamment nécessaires si l'on change 
    /// de modèles 3D.
    /// </summary>
    [Header("Adjustment")]
    
    [SerializeField] int heightPiton;    
    [SerializeField] float heightRobot;
    [SerializeField] float heightDiamond;
    [SerializeField] float heightTag;

    #endregion

    #region Robot

    /// <summary>
    /// Regroupe les champs sérialisés liés au Robot. Il seront transmis à l'instance du robot.
    /// </summary>
    [Header("Robot")]

    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] [Range(0.001f, 0.02f)] float robotSpeed;

    #endregion

    #endregion

    #region Private

    realRobot myRobot;

    #endregion

    #endregion





    void Start()
    {
        
        #region Check


        /// Il s'agit ici de faire toutes les vérifications sur les variables entrées 
        /// par l'utilisateur(informaticien) dans les champs sérialisés afin d'éviter des erreurs.
        /// On utilise une classe statique regroupant ces fonctions, la classe Utile.
        Utile.checkNotNull(cellAsset, robotAsset, thePlanet, refCamera, diamondAsset,
            tagAsset, explosionEffect, materialGrid, materialOriginalCell);

        Utile.checkNotNull(pitonElements);

        Utile.checkNotZeroOrLess(minDimensionMap.x, minDimensionMap.y, maxDimensionMap.x,maxDimensionMap.y,
            heightPiton);

        Utile.checkNotNegative(initialPositionRobot.x , initialPositionRobot.y);

        #endregion


        #region Assign

        ///transmission des données (map) à la planète par un objet Initialization
        ///afin de respecter l'interface commune notamment
        Initialization init = new Initialization(minDimensionMap, maxDimensionMap, initialPositionRobot,
            isRandomInitialPosXRobot, isRandomInitialPosYRobot, positionDiamond, randomPosDiamond,
            orientationRobot, randomOrientationRobot, isInCorner, cornerRobot);


        ///Transmission des données (map) à la planète/instance de planète
        thePlanet.Initialize(init);

        ///Transmission des données liées à la 3D (Objets 3D notamment) à la planète
        ///Cette fois-ci sans utiliser d'objet (pas d'interface à respecter)
        thePlanet.Initialize3D(cellAsset, robotAsset, thePlanet,
        diamondAsset, pitonElements, initialPosMap, refCamera, heightPiton,
        heightRobot, heightDiamond, tagAsset, heightTag, materialOriginalCell, materialGrid);


        myRobot = thePlanet.myRobot;

        ///Transmission des données au robot (3D/programmation confondues)
        myRobot.Initialize(thePlanet, explosionEffect, robotSpeed);




        #endregion


    }


    /// <summary>
    /// Fonction appelée par le clic sur le bouton.
    /// Permet d'afficher/supprimer le dallage grille.
    /// </summary>
    public void eventGrid()
    {
        thePlanet.showGrid();
    }
}