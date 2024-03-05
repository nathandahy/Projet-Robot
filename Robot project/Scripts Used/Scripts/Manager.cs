using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui servira de point d'entr�e unique pour les champs s�rialis�s.
/// R�partit les donn�es entre Planet et Robot
/// </summary>
public class Manager : MonoBehaviour
{

    #region Variables

    #region Serialize

    #region References


    /// <summary>
    /// Regroupement des champs s�rialis�s li�s � des r�f�rences sur des objets.
    /// Tous ces objets sont n�cessaires au d�roulement du programme
    /// </summary>
    [Header("References/3D Assets")]
    [SerializeField] realCell cellAsset;  // CellAsset, robotAsset et thePlanet d�signe un script/instance de classe,
    [SerializeField] realRobot robotAsset; // mais ce script doit �tre attach� � l'objet 3d voulu pour le repr�senter
    [SerializeField] realPlanet thePlanet; // il faut donc un objet ayant un composant script.
    [SerializeField] GameObject diamondAsset;
    [SerializeField] GameObject tagAsset;
    [SerializeField] Transform initialPosMap;
    [SerializeField] Camera refCamera;
    [SerializeField][Tooltip("Peut contenir un nombre aussi grand que voulu d'�l�ments 3D")] 
    GameObject[] pitonElements; 



    #endregion


    #region Planet


    /// <summary>
    /// Regroupe tous les champs s�rialis�s qui seront transmis � Planet
    /// </summary>
    [Header("Planet")]
    [SerializeField] Vector2Int minDimensionMap;  //Pour des dimensions al�atoires, on personnalisera ces bornes
    [SerializeField] Vector2Int maxDimensionMap; // Pour des dimensions fixes on met les m�mes valeurs dans ces deux champs
    [SerializeField] Vector2Int initialPositionRobot;
    [SerializeField] bool isRandomInitialPosXRobot; // On peut choisir une colonne fixe et une ligne al�atoire
    [SerializeField] bool isRandomInitialPosYRobot; // D'o� la s�paration en deux champs distincts
    [SerializeField] Vector2Int positionDiamond;
    [SerializeField] bool randomPosDiamond; // La position al�atoire du diamant n'est elle pas divis�e en deux champs. 
                                            // Colonne et ligne sont fixes ou sont al�atoires les deux.
    [SerializeField] Direction orientationRobot; 
    [SerializeField] bool randomOrientationRobot; // si coch� on ne prendra pas en compte orientationRobot
    [SerializeField] bool isInCorner;
    [SerializeField] Corner cornerRobot; // si isInCorner est coch�, on place le robot dans le coin indiqu�. Sinon n'est pas activ�
    [SerializeField] Material materialOriginalCell;
    [SerializeField] Material materialGrid;

    #endregion


    #region Adjustment

    /// <summary>
    /// Regroupe tous les champs s�rialis�s li�s � des ajustements.
    /// On trouve surtout des r�glages li�s � la hauteur des mod�les 3D par rapport 
    /// � la planet (map). Des changements seront notamment n�cessaires si l'on change 
    /// de mod�les 3D.
    /// </summary>
    [Header("Adjustment")]
    
    [SerializeField] int heightPiton;    
    [SerializeField] float heightRobot;
    [SerializeField] float heightDiamond;
    [SerializeField] float heightTag;

    #endregion

    #region Robot

    /// <summary>
    /// Regroupe les champs s�rialis�s li�s au Robot. Il seront transmis � l'instance du robot.
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


        /// Il s'agit ici de faire toutes les v�rifications sur les variables entr�es 
        /// par l'utilisateur(informaticien) dans les champs s�rialis�s afin d'�viter des erreurs.
        /// On utilise une classe statique regroupant ces fonctions, la classe Utile.
        Utile.checkNotNull(cellAsset, robotAsset, thePlanet, refCamera, diamondAsset,
            tagAsset, explosionEffect, materialGrid, materialOriginalCell);

        Utile.checkNotNull(pitonElements);

        Utile.checkNotZeroOrLess(minDimensionMap.x, minDimensionMap.y, maxDimensionMap.x,maxDimensionMap.y,
            heightPiton);

        Utile.checkNotNegative(initialPositionRobot.x , initialPositionRobot.y);

        #endregion


        #region Assign

        ///transmission des donn�es (map) � la plan�te par un objet Initialization
        ///afin de respecter l'interface commune notamment
        Initialization init = new Initialization(minDimensionMap, maxDimensionMap, initialPositionRobot,
            isRandomInitialPosXRobot, isRandomInitialPosYRobot, positionDiamond, randomPosDiamond,
            orientationRobot, randomOrientationRobot, isInCorner, cornerRobot);


        ///Transmission des donn�es (map) � la plan�te/instance de plan�te
        thePlanet.Initialize(init);

        ///Transmission des donn�es li�es � la 3D (Objets 3D notamment) � la plan�te
        ///Cette fois-ci sans utiliser d'objet (pas d'interface � respecter)
        thePlanet.Initialize3D(cellAsset, robotAsset, thePlanet,
        diamondAsset, pitonElements, initialPosMap, refCamera, heightPiton,
        heightRobot, heightDiamond, tagAsset, heightTag, materialOriginalCell, materialGrid);


        myRobot = thePlanet.myRobot;

        ///Transmission des donn�es au robot (3D/programmation confondues)
        myRobot.Initialize(thePlanet, explosionEffect, robotSpeed);




        #endregion


    }


    /// <summary>
    /// Fonction appel�e par le clic sur le bouton.
    /// Permet d'afficher/supprimer le dallage grille.
    /// </summary>
    public void eventGrid()
    {
        thePlanet.showGrid();
    }
}