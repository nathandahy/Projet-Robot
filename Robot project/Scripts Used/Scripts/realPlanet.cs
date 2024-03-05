using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Impl�mentation 3D concr�te de l'interface Planet commune.
/// G�re donc la map au niveau traitements mais aussi sa construction 3D
/// </summary>
public class realPlanet : MonoBehaviour, Planet
{

    #region Variables



    /// <summary>
    /// Les variables utilis�s sont moins nombreuses que celles transmises par l'objet initialization.
    /// Certaines servaient � un calcul et ne sont pas n�cessaires.
    /// Exemple: dimensionMap a �t� calcul�e � partir de minDimensionMap et maxDimension map de l'objet initialization
    /// </summary>

    #region Robot
    [HideInInspector] public realRobot myRobot;

    Vector2Int dimensionMap;
    Vector2Int positionRobot;
    Vector2Int positionDiamond;
    Direction orientationRobot;

    #endregion

    #region map

    realCell[,] map;  //la carte est donc une matrice de cellules (objets)
    Transform initialPosMap;

    #endregion

    #region Others

    Camera refCamera;

    GameObject tagAsset;
    float heightTag; 

    Material originalMaterial;
    Material materialGrid;

    bool grid = false;

    #endregion


    #endregion

    #region Interfaces Implementation

    /// <summary>
    /// Impl�ment�e pour respecter l'interface commune, n'est pas utilis�e ici
    /// </summary>
    /// <param name="spawnID"></param>
    public void BoardSetup(int spawnID) { }

    /// <summary>
    /// Retourne la dimension de la carte (ligne et colonne)
    /// </summary>
    /// <returns>On utilise un Tuple pour respecter l'interface, un Vector2Int aurait �t� possible</returns>
    public System.Tuple<int, int> Dimension()
    {
        return new System.Tuple<int, int>(dimensionMap.x, dimensionMap.y);
    }

    /// <summary>
    /// Permet de modifier la dimension de la map. 
    /// Non utilis�e dans ce programme
    /// </summary>
    public void SetDimension(int column, int row)
    {
        dimensionMap.x = column;
        dimensionMap.y = row;
    }

    /// <summary>
    /// Permet de r�cup�rer l'orientation du robot
    /// </summary>
    /// <param name="robot">Param�tre n'est pas utilis�, est ici pour respecter l'interface</param>
    public Direction Orientation(Robot robot)
    {
        return orientationRobot;
    }

    /// <summary>
    /// Permet de changer l'orientation du robot
    /// </summary>
    /// <param name="robot">Param�tre n'est pas utilis�, est ici pour respecter l'interface</param>
    public void SetOrientation(Robot robot, Direction orientation)
    {
        orientationRobot = orientation;
    }

    /// <summary>
    /// Permet de r�cup�rer la position du robot sur la map
    /// </summary>
    /// <param name="robot">Param�tre n'est pas utilis�, est ici pour respecter l'interface</param>
    public Vector2 Position(Robot robot)
    {       
        return positionRobot;
    }

    /// <summary>
    /// Permet de changer la position du robot sur la map
    /// Plusieurs v�rifications sont faites pour voir si le robot est sorti du terrain
    /// </summary>
    /// <param name="robot">Param�tre n'est pas utilis�, est ici pour respecter l'interface</param>
    public void SetPosition(Robot robot, Vector2 position)
    {
        Vector2Int castPosition = new Vector2Int(0,0);
        castPosition.x = (int)position.x;
        castPosition.y = (int)position.y;
        positionRobot = castPosition;


        if (position.x > dimensionMap.x - 1)
        {
            setDead();

        }
        else if (position.x < 0)
        {
            setDead();

        }

        if (position.y > dimensionMap.y - 1)
        {
            setDead();

        }
        else if (position.y < 0)
        {
            setDead();

        }
    }

    /// <summary>
    /// Permet de r�cup�rer une cellule. Celle-ci contient des informations utiles (Mark, Ore)
    /// </summary>
    public Cell GetCell(int column, int row)
    {
        return new realCell();
    }



    #endregion

    #region Others Functions

    /// <summary>
    /// Fonction utilitaire mais non plac� dans Utile car sp�cifique.
    /// Permet de transformer un Vector2 (n�cessaire pour les interfaces) en Vector2Int
    /// </summary>
    public Vector2Int PositionInt()
    {
        Vector2Int castPosition = new Vector2Int(0, 0);
        castPosition.x = (int)positionRobot.x;
        castPosition.y = (int)positionRobot.y;

        return castPosition;
    }

    /// <summary>
    /// Marque la case de la map. Si d�j� marqu�e/contient un diamant,
    /// le marquage est annul� (Destroy)
    /// </summary>
    public void SetMark()
    {
        GameObject tag = Instantiate(tagAsset, new Vector3(initialPosMap.position.x, initialPosMap.position.y, initialPosMap.position.z), Quaternion.Euler(0, 0, 0));
        Vector3 interPosition3 = map[positionRobot.x, positionRobot.y].gameObject.GetComponent<Transform>().position;
        tag.gameObject.GetComponent<Transform>().position = new Vector3(interPosition3.x, interPosition3.y + heightTag, interPosition3.z);
  
        if (map[positionRobot.x, positionRobot.y].Mark(tag) == false)
        {
            Destroy(tag);
        }
       
        
    }

    /// <summary>
    /// Supprime la marque courante sur cette case
    /// </summary>
    public void DeleteMark()
    {
        map[positionRobot.x, positionRobot.y].DeleteMark();
    }

    /// <summary>
    /// Change une cellule
    /// </summary>
    public void SetCell(int column, int row, realCell newCell)
    {
        map[column, row] = newCell;
    }

    /// <summary>
    /// Rend le robot cass�. Implique des traitements 3D ici (cam�ra) et dans le robot.
    /// </summary>
    void setDead()
    {
        
        myRobot.dead = true;

        refCamera.GetComponent<Transform>().LookAt(myRobot.gameObject.transform);

    }

    /// <summary>
    /// Calcule si le robot est en face d'une marque
    /// </summary>
    public bool isFrontOfMark()
    {
       
        if(orientationRobot == Direction.North && map[positionRobot.x,positionRobot.y +1].isMarked)
        {
            return true;
        }
        if (orientationRobot == Direction.East && map[positionRobot.x -1, positionRobot.y].isMarked)
        {
            return true;
        }
        if (orientationRobot == Direction.South && map[positionRobot.x, positionRobot.y -1].isMarked)
        {
            return true;
        }
        if (orientationRobot == Direction.West && map[positionRobot.x+1, positionRobot.y ].isMarked)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// Calcule si le robot est sur une marque
    /// </summary>
    public bool isOnMark()
    {
        return map[positionRobot.x, positionRobot.y].isMarked;
    }

    /// <summary>
    /// Calcule si le robot est en face d'un diamant
    /// </summary>
    public bool IsFrontOfOre()
    {
        if (orientationRobot == Direction.North && map[positionRobot.x, positionRobot.y + 1].isOred)
        {
            return true;
        }
        if (orientationRobot == Direction.East && map[positionRobot.x - 1, positionRobot.y].isOred)
        {
            return true;
        }
        if (orientationRobot == Direction.South && map[positionRobot.x, positionRobot.y - 1].isOred)
        {
            return true;
        }
        if (orientationRobot == Direction.West && map[positionRobot.x + 1, positionRobot.y].isOred)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// Calcule si le robot est sur un diamant
    /// </summary>
    public bool IsOnOre()
    {
        return map[positionRobot.x, positionRobot.y].isOred;
    }

    /// <summary>
    /// Calcule si le robot se trouve en face d'un trou (bord de la map)
    /// </summary>
    public bool IsInFrontOfDanger()
    {
        if (orientationRobot == Direction.North &&  (positionRobot.y + 1 >= dimensionMap.y))
        {
            return true;
        }
        if (orientationRobot == Direction.East && (positionRobot.x <= 0))
        {
            return true;
        }
        if (orientationRobot == Direction.South && (positionRobot.y <= 0))
        {
            return true;
        }
        if (orientationRobot == Direction.West && (positionRobot.x + 1 >= dimensionMap.x))
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Initializing Functions

    /// <summary>
    /// Impl�mentation de la m�thode Initialize de l'interface commune
    /// Proc�de � la mise en place de la plan�te/map d'un point de vue programmation.
    /// Et ce � partir des donn�es envoy�es par le manager
    /// Vue la longueur des traitements � faire, elle est subdivis�e en sous-fonctions.
    /// </summary>
    /// <param name="init">N�cessite l'objet init contenant toutes les informations</param>
    public void Initialize(Initialization init)
    {
        initializeMap(init);
        initializePosRobot(init);
        initializeCornerRobot(init);
        initializeOrientationRobot(init);
        initializeDiamond(init);
    }

    /// <summary>
    /// Fonction transmettant du manager � la plan�te les donn�es li�es � la repr�sentation 3D
    /// On retrouve donc une s�paration 
    /// Initialize -> traitements, commun avec la version 2D, donc venant de l'interface
    /// Et Initialize3D -> repr�sentation 3D, propre � cette classe
    /// Vu la longueur des traitements elle est subdivis�e en sous-fonctions.
    /// </summary>
    public void Initialize3D(realCell cellAsset, realRobot robotAsset, realPlanet planetAsset,
        GameObject diamondAsset, GameObject[] pitonElements, Transform initialPosMap
        , Camera refCamera, int heightPiton, float heightRobot, float heightDiamond, GameObject tagAsset,
        float heightTag, Material originalMaterial, Material materialGrid)
    {
        initialize3DMap(cellAsset, initialPosMap,pitonElements,heightPiton);
        initialize3DRobot(robotAsset, initialPosMap, heightRobot);
        initialize3DCamera(refCamera);
        initialize3DDiamond(diamondAsset, initialPosMap, heightDiamond);
        initialize3DTag(tagAsset, heightTag);

        this.originalMaterial = originalMaterial;
        this.materialGrid = materialGrid;
        
    }

    #endregion

    #region SubFunctions Initialize (Process)

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te
    /// Gerant les dimensions de la carte
    /// Cr�e une matrice d'objet Cell repr�sentant la map
    /// </summary>
    /// <param name="init">N�cessite l'objet init contenant les informations</param>
    void initializeMap(Initialization init)
    {
        dimensionMap.x = Random.Range(init.minDimensionMap.x, init.maxDimensionMap.x);
        dimensionMap.y = Random.Range(init.minDimensionMap.y, init.maxDimensionMap.y);

        map = new realCell[dimensionMap.x, dimensionMap.y];
    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te
    /// Gerant la position du robot
    /// </summary>
    /// <param name="init">N�cessite l'objet init contenant les informations</param>    
    void initializePosRobot(Initialization init)
    {

        positionRobot.x = init.initialPositionRobot.x;
        positionRobot.y = init.initialPositionRobot.y;

        if (positionRobot.x > dimensionMap.x - 1)
        {
            positionRobot.x = dimensionMap.x - 1;
        }

        if (positionRobot.y > dimensionMap.y - 1)
        {
            positionRobot.y = dimensionMap.y - 1;
        }


        if (init.isRandomInitialPosXRobot == true)
        {
            positionRobot.x = Random.Range(0, dimensionMap.x - 1);
        }

        if (init.isRandomInitialPosYRobot == true)
        {
            positionRobot.y = Random.Range(0, dimensionMap.y - 1);
        }
    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te
    /// G�rant le placement �ventuel du robot dans un coin
    /// </summary>
    /// <param name="init">N�cessite l'objet init contenant les informations</param> 
    void initializeCornerRobot(Initialization init)
    {
        if (init.isInCorner == true)
        {
            if (init.cornerRobot == Corner.NorthWest)
            {
                positionRobot.x = dimensionMap.x - 1;
                positionRobot.y = dimensionMap.y - 1;
            }
            else if (init.cornerRobot == Corner.NorthEast)
            {
                positionRobot.x = 0;
                positionRobot.y = dimensionMap.y - 1;
            }
            else if (init.cornerRobot == Corner.SouthEast)
            {
                positionRobot.x = 0;
                positionRobot.y = 0;
            }
            else if (init.cornerRobot == Corner.SouthWest)
            {
                positionRobot.x = dimensionMap.x - 1;
                positionRobot.y = 0;
            }
        }
    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te
    /// Gerant l'orientation du robot
    /// </summary>
    /// <param name="init">N�cessite l'objet init contenant les informations</param> 
    void initializeOrientationRobot(Initialization init)
    {      

        if (init.randomOrientationRobot == true)
        {
            int i = Random.Range(0, 4);

            switch (i)
            {
                case 1:
                    orientationRobot = Direction.West;
                    break;
                case 2:
                    orientationRobot = Direction.North;
                    break;
                case 3:
                    orientationRobot = Direction.East;
                    break;
                case 4:
                    orientationRobot = Direction.South;
                    break;

            }

        }
        else
        {
            orientationRobot = init.orientationRobot;
        }
    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te
    /// Gerant la position du diamant
    /// </summary>
    /// <param name="init">N�cessite l'objet init contenant les informations</param> 
    void initializeDiamond(Initialization init)
    {

        positionDiamond.x = init.positionDiamond.x;
        positionDiamond.y = init.positionDiamond.y;



        if (init.randomPosDiamond == true)
        {
            positionDiamond.x = Random.Range(0, dimensionMap.x - 1);
            positionDiamond.y = Random.Range(0, dimensionMap.y - 1);
        }

        if (positionDiamond.x > dimensionMap.x - 1)
        {
            positionDiamond.x = dimensionMap.x - 1;
        }

        if (positionDiamond.y > dimensionMap.y - 1)
        {
            positionDiamond.y = dimensionMap.y - 1;
        }
    }

    #endregion

    #region SubFunctions Initialize3D 

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te en 3D
    /// Gerant le robot :
    /// position initiale, orientation, coin ou non : du mod�le 3D
    /// A noter l'utilisation d'une fonction qui factorise certains traitements:
    /// factorOrientationRobot3D
    /// </summary>
    void initialize3DRobot(realRobot robotAsset, Transform initialPosMap, float heightRobot)
    {
        robotAsset = Instantiate(robotAsset,
            new Vector3(initialPosMap.position.x, initialPosMap.position.y + heightRobot, initialPosMap.position.z),
            Quaternion.Euler(0, 0, 0));

        myRobot = robotAsset;
        //setPositionRobotX(posXInitialeRobot);

        //setPositionRobotY(posYInitialeRobot);

        if (orientationRobot == Direction.North)
        {
            //Pas de modifications d'orientation
        }
        else if (orientationRobot == Direction.East)
        {
            factorOrientationRobot3D(robotAsset, 90);
        }
        else if (orientationRobot == Direction.South)
        {
            factorOrientationRobot3D(robotAsset, 180);
        }
        else if (orientationRobot == Direction.West)
        {
            factorOrientationRobot3D(robotAsset, 270);
        }

        
        Vector3 interPosition2 = map[positionRobot.x, positionRobot.y].gameObject.GetComponent<Transform>().position;
        robotAsset.gameObject.GetComponent<Transform>().position = new Vector3(interPosition2.x, robotAsset.gameObject.GetComponent<Transform>().position.y, interPosition2.z);
        
    }

    /// <summary>
    /// Fonction de factorisation du traitement sur les rotations
    /// de la fonction Initialize3DRobot
    /// </summary>
    /// <param name="rotation">Rotation suppl�mentaire � appliquer sur le robot 3D</param>
    void factorOrientationRobot3D(realRobot robotAsset, int rotation)
    {
        // Il y a n�cessit� d'utiliser eulerAngles et Quaternion.Euler pour modifier des rotations d'objets

        Vector3 interRotation;

        interRotation = robotAsset.transform.rotation.eulerAngles; 
        interRotation.y += rotation;
        robotAsset.transform.rotation = Quaternion.Euler(interRotation);
    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te en 3D
    /// Gerant la map :
    /// Placement des cases, et du piton en 3D de fa�on proc�durale (en fct de la taille de la map)
    /// A noter l'utilisation de deux fonctions de factorisation pour la g�n�ration du d�cor du piton
    /// </summary>
    void initialize3DMap(realCell cellAsset, Transform initialPosMap, GameObject[] pitonElements, float heightPiton)
    {
        for (int i = 0; i < dimensionMap.x; i++)
        {
            for (int j = 0; j < dimensionMap.y; j++)
            {
                #region G�n�ration de la Map en 3D

                this.initialPosMap = initialPosMap;

                // Instantiation 3D et objet d'une case
                realCell interCell = Instantiate(cellAsset, initialPosMap.position, Quaternion.Euler(0, 0, 0));
                

                //r�cup�ration en r�f�rence pour simplifier l'�criture qui va suivre (et la compr�hension)
                Transform transformInterCell = interCell.GetComponent<Transform>();

                // placement dans l'espace 3D
                transformInterCell.position = new Vector3(transformInterCell.position.x - 1 * i, 
                    transformInterCell.position.y, transformInterCell.position.z + 1 * j);

                //affectation dans notre matrice d'objets Cell
                map[i, j] = interCell;

                #endregion



                #region G�n�ration du d�cor du piton


                //Les cas suivants correspondent aux extr�mit�s du terrain donc on y g�n�re nos d�cors piton:

                if (i == 0)
                {
                    factorInitializePitonTop(pitonElements, initialPosMap, i, j, 0.7f, 0, 0, 0, 90);
                }


                if (j == 0)
                {
                    factorInitializePitonTop(pitonElements, initialPosMap, i, j, 0, -0.7f, 0, 90, 90);
                }

                if (i == dimensionMap.x - 1)
                {
                    factorInitializePitonTop(pitonElements, initialPosMap, i, j, -0.7f,0, 0, 0, 90);
                }


                if (j == dimensionMap.y - 1)
                {
                    factorInitializePitonTop(pitonElements, initialPosMap, i, j, 0, 0.7f, 0, 90, 90);              
                }





                for (int k = 1; k <= heightPiton; k++)
                {


                    // construction du piton central par ajout de plusieurs �tages �gaux � la map
                    //cependant on ne les ajoute pas � une matrice: ce ne sont que des mod�les 3D 
                    // GameObject et non realCell.
                    GameObject pitonBase = Instantiate(cellAsset.gameObject, initialPosMap.position, Quaternion.Euler(0, 0, 0));

                    Transform interPitonBaseTransform =  pitonBase.GetComponent<Transform>();
                    interPitonBaseTransform.position = new Vector3(interPitonBaseTransform.position.x - 1 * i, interPitonBaseTransform.position.y - k, interPitonBaseTransform.position.z + 1 * j);



                    //Les cas suivants correspondent aux extr�mit�s du terrain donc on y g�n�re nos d�cors piton:

                    if (i == 0)
                    {
                        factorInitializePitonSide(pitonElements, initialPosMap, i, j, k, 0.7f, 0, 0);
                    }


                    if (j == 0)
                    {
                        factorInitializePitonSide(pitonElements, initialPosMap, i, j, k, 0, -0.7f, 90);                
                    }

                    if (i == dimensionMap.x - 1)
                    {
                        factorInitializePitonSide(pitonElements, initialPosMap, i, j, k, -0.7f,0,0);                
                    }


                    if (j == dimensionMap.y - 1)
                    {
                        factorInitializePitonSide(pitonElements, initialPosMap, i, j, k, 0, 0.7f, 90);                    
                    }
                }


                #endregion

            }

        }
    }

    /// <summary>
    /// Fonction de factorisation pour les objets d�cor du piton se trouvant au sommet (leur g�n�ration)
    /// </summary>
    void factorInitializePitonTop(GameObject[] pitonElements, Transform initialPosMap, int i, int j, float ecartX, float ecartZ, float ajoutRotX, float ajoutRotY, float ajoutRotZ) 
    {
        int pitonRand = Random.Range(0, pitonElements.Length); //permet de donner de la vari�t� et de l'al�atoire au piton construit
        GameObject interPitonElement = Instantiate(pitonElements[pitonRand].gameObject, initialPosMap.position, Quaternion.Euler(0, 0, 0));
        interPitonElement.GetComponent<Transform>().position = new Vector3(interPitonElement.GetComponent<Transform>().position.x - 1 * i + ecartX, interPitonElement.GetComponent<Transform>().position.y, interPitonElement.GetComponent<Transform>().position.z + 1 * j + ecartZ);
        interPitonElement.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(interPitonElement.GetComponent<Transform>().rotation.x + ajoutRotX, interPitonElement.GetComponent<Transform>().rotation.y + ajoutRotY, interPitonElement.GetComponent<Transform>().rotation.z + ajoutRotZ));

        interPitonElement.GetComponent<Collider>().enabled = false;  // il ne faut pas que le robot puisse entre en collision avec ces objets
        interPitonElement.GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);  // les pitons au sommet sont moins grands
    }

    /// <summary>
    /// Fonction de factorisation pour les objets d�cor du piton se trouvant sur les c�t�s/�tages (leur g�n�ration)
    /// La diff�rence principale avec l'autre fonction de factorisation est:
    /// Ajout du param�tre k ; hauteur dans l'espace
    /// Et pas de r�duction de la taille des pitons (ceux des c�t�s sont plus gros)
    /// </summary>
    void factorInitializePitonSide(GameObject[] pitonElements, Transform initialPosMap, int i, int j, int k, float ecartX, float ecartZ, float ajoutRotY)
    {
        int pitonRand = Random.Range(0, pitonElements.Length);
        GameObject interPitonElement = Instantiate(pitonElements[pitonRand].gameObject, initialPosMap.position, Quaternion.Euler(0, 0, 0));
        interPitonElement.GetComponent<Transform>().position = new Vector3(interPitonElement.GetComponent<Transform>().position.x - 1 * i + ecartX, interPitonElement.GetComponent<Transform>().position.y - k, interPitonElement.GetComponent<Transform>().position.z + 1 * j + ecartZ);
        interPitonElement.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(interPitonElement.GetComponent<Transform>().rotation.x + Random.Range(-20f, 20f), interPitonElement.GetComponent<Transform>().rotation.y + ajoutRotY + Random.Range(-20f, 20f), interPitonElement.GetComponent<Transform>().rotation.z + Random.Range(70f, 120f)));
    }




    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te en 3D
    /// Gerant la cam�ra : 
    /// placement et orientation en fonction de la taille de la map
    /// </summary>
    void initialize3DCamera(Camera refCamera)
    {
        this.refCamera = refCamera;

        Transform interCam = refCamera.GetComponent<Transform>(); 
        // stockage pour des questions de facilit� de lecture (et de performance)


        //replacement de la cam�ra en trois fois pour plus de clart� (un par axe)
        //on replace la cam�ra selon la taille de la map. Les valeurs d'ajout peuvent �tre chang�es 
        //pour �quilibrer le placement de la cam�ra

       
        interCam.position = new Vector3(interCam.position.x, interCam.position.y + 0.4f * dimensionMap.x + 0.5f * dimensionMap.y, interCam.position.z);


        interCam.position = new Vector3(interCam.position.x - 0.43f * dimensionMap.x, interCam.position.y, interCam.position.z);

        interCam.position = new Vector3(interCam.position.x, interCam.position.y, interCam.position.z + 0.5f * dimensionMap.y);


        //la position de la cam�ra �tait faisable de fa�on lin�aire. Ce n'est pas le cas de la rotation
        // de la cam�ra. Plut�t que d'utiliser des formules complexes
        // on oriente la cam�ra vers le centre de la map.

        interCam.LookAt(map[dimensionMap.x / 2, dimensionMap.y / 2].gameObject.GetComponent<Transform>());
        
        // on veut seulement modifier la rotation bas-haut et garder la cam�ra droite:
        interCam.rotation = Quaternion.Euler(new Vector3(interCam.rotation.eulerAngles.x, 0f, 0f)); 

    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te en 3D
    /// Gerant le diamant:
    /// sa position sur la map
    /// </summary>
    void initialize3DDiamond(GameObject diamondAsset, Transform initialPosMap, float heightDiamond)
    {
        GameObject diamond = Instantiate(diamondAsset, initialPosMap.position, Quaternion.Euler(0, 0, 0));

        Transform diamondTransform =  diamond.GetComponent<Transform>();
        diamondTransform.position = new Vector3(diamondTransform.position.x - 1 * positionDiamond.x, diamondTransform.position.y + heightDiamond, diamondTransform.position.z + 1 * positionDiamond.y);
        map[positionDiamond.x, positionDiamond.y].isOred = true;
    
    }

    /// <summary>
    /// Sous-Fonctions d'initialisation de la plan�te en 3D
    /// G�rant les marques
    /// </summary>

    void initialize3DTag(GameObject tagAsset, float heightTag)
    {
        this.tagAsset = tagAsset;
        this.heightTag = heightTag;
    }

    #endregion

    /// <summary>
    /// Permet d'afficher un dallage en damier pour mettre en �vidence les cases
    /// </summary>
    public void showGrid()
    {

        grid = !grid;

        if (grid == true)
        {
            
            int plus = 0; //d�calage n�cessaire par ligne

            for (int i = 0; i < dimensionMap.x; i++)
            {
                for (int j = 0; j < dimensionMap.y; j++)
                {
                    if (j % 2 == 0) //on se d�cale � chaque ligne
                    {
                        plus = 0;
                    }
                    else
                    {
                        plus = 1;
                    }

                    if ((i + plus) % 2 == 0) //une case sur deux
                    {
                        map[i, j].gameObject.GetComponent<Renderer>().material = materialGrid; 
                        //mat�riel de marquage
                    }
                }
            }
        }
        else //si d�sactivation de la grille on repasse tout � l'ancien materiel.
        {
            for (int i = 0; i < dimensionMap.x; i++)
            {
                for (int j = 0; j < dimensionMap.y; j++)
                {
                    map[i, j].gameObject.GetComponent<Renderer>().material = originalMaterial;
                    
                }
            }
        }
    }
}

