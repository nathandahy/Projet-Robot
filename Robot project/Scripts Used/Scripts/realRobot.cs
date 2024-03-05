using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realRobot : MonoBehaviour, Robot
{

    #region Private


    Transform robotTransform;
    public bool dead = false;

    float objectifX = 0;
    float objectifZ = 0;

    Animator anim;

    Vector3 interRotation;
    Vector3 interPosition;

    float robotSpeed;

    ParticleSystem explosionEffect;

    realPlanet thePlanet;
   

    #endregion


    private void Awake()
    {
        robotTransform = this.gameObject.GetComponent<Transform>();
    }

    private void Start()
    {
        
        objectifX = gameObject.transform.position.x;
        objectifZ = gameObject.transform.position.z;

      

        anim = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// Pour bouger le robot calcule sa prochaine position
    /// et y arrive de façon progressive
    /// </summary>
    public void Move()
    {
        // si le robot n'est pas déjà en train de bouger -> bloquage par mouvement
        // Approximately nous dit si des flottants sont proches
        if (Mathf.Approximately(objectifZ, gameObject.transform.position.z)  && Mathf.Approximately(objectifX, gameObject.transform.position.x) )
        {
            if (thePlanet.Orientation(this) == Direction.North)
            {
                thePlanet.SetPosition(this, new Vector2(thePlanet.Position(this).x, thePlanet.Position(this).y + 1));
                interPosition = gameObject.transform.position;

                objectifZ = interPosition.z + 1f;
            }
            else if (thePlanet.Orientation(this) == Direction.East)
            {
                thePlanet.SetPosition(this, new Vector2(thePlanet.Position(this).x - 1, thePlanet.Position(this).y));

                objectifX = interPosition.x + 1f;
            }
            else if (thePlanet.Orientation(this) == Direction.South)
            {
                thePlanet.SetPosition(this, new Vector2(thePlanet.Position(this).x, thePlanet.Position(this).y - 1));


                objectifZ = interPosition.z - 1f;
            }
            else if (thePlanet.Orientation(this) == Direction.West)
            {
                thePlanet.SetPosition(this, new Vector2(thePlanet.Position(this).x + 1, thePlanet.Position(this).y));
                interPosition = gameObject.transform.position;

                objectifX = interPosition.x - 1f;

            }
        }

        //on vérifie les éléments alentours (diamant, marque, danger)
        verifyAround();
        
    }

    /// <summary>
    /// fait tourner le robot (un seul sens de rotation), aussi bien en traitements
    /// qu'en 3D
    /// </summary>
    public void Rotate()
    {
        //bloquage si mouvement en cours
        if (Mathf.Approximately(objectifZ, gameObject.transform.position.z) && Mathf.Approximately(objectifX, gameObject.transform.position.x))
        {
            if (thePlanet.Orientation(this) == Direction.North)
            {
                thePlanet.SetOrientation(this, Direction.East);
            }
            else if (thePlanet.Orientation(this) == Direction.East)
            {
                thePlanet.SetOrientation(this, Direction.South);
            }
            else if (thePlanet.Orientation(this) == Direction.South)
            {
                thePlanet.SetOrientation(this, Direction.West);
            }
            else if (thePlanet.Orientation(this) == Direction.West)
            {
                thePlanet.SetOrientation(this, Direction.North);
            }


            interRotation = gameObject.transform.rotation.eulerAngles;
            interRotation.y += 90;
            gameObject.transform.rotation = Quaternion.Euler(interRotation);

            //vérification des éléments alentours
            verifyAround();
        }
    }

    /// <summary>
    /// Pose une marque à la position actuelle du robot
    /// </summary>
    public void Mark()
    {
        thePlanet.SetMark();
        IsOnMark();
    }

    /// <summary>
    /// supprime la marque (si elle existe) à la position actuelle du robot
    /// </summary>
    public void DeleteMark()
    {
        thePlanet.DeleteMark();
    }

    /// <summary>
    /// Nous dit si le robot est en face d'une marque
    /// </summary>
    public bool isFrontOfMark()
    {
        if(thePlanet.isFrontOfMark())
        {
            Utile.show("Vous etes en face d'une marque");
            return true;
        }
        else
        {
            return false;
        }
        
    }

    /// <summary>
    /// Nous dit si le robot est sur une marque
    /// </summary>
    public bool IsOnMark()
    {
        if (thePlanet.isOnMark())
        {
            Utile.show("Vous etes sur une marque");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Nous dit si le robot est en face d'un diamant
    /// </summary>
    public bool IsFrontOfOre()
    {
        if(thePlanet.IsFrontOfOre())
        {
            Utile.show("Vous etes en face d'un diamant");
            return true;
        }
        else
        {
            return false;
        }
        
    }

    /// <summary>
    /// Nous dit si le robot est sur un diamant
    /// </summary>
    /// <returns></returns>
    public bool IsOnOre()
    {
        if (thePlanet.IsOnOre())
        {
            Utile.show("Vous etes sur un diamant");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Nous dit si le robot est au bord du piton
    /// </summary>
    public bool IsInFrontOfDanger()
    {
        if (thePlanet.IsInFrontOfDanger())
        {
            Utile.show("Vous etes au bord du piton");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Donne la vitesse du robot, non utilisée dans cette implémentation
    /// </summary>
    public float Speed()
    {
        return robotSpeed;
    }

    /// <summary>
    /// Change la vitesse du robot
    /// </summary>
    public void SetSpeed(float speed)
    {
        this.robotSpeed = speed;
    }

    /// <summary>
    /// dit si le robot est cassé
    /// </summary>
    public bool isBroken()
    {
        return dead;
    }

    /// <summary>
    /// change l'objet planet
    /// </summary>
    public void SetPlanet(Planet planet)
    {
        thePlanet = (realPlanet)planet;
    }

    /// <summary>
    /// S'exécute à chaque frame, on réagit ici aux commandes de l'utilisateur
    /// Et on fait progresser le mouvement du robot
    /// </summary>
    private void Update()
    {
        if (!dead) // si le robot est mort le jeu s'arrete
        {
            if (Input.GetKeyDown("z"))
            {
                Move();
            }

            if (Input.GetKeyDown("d"))
            {
                Rotate();
            }


            if (Input.GetKeyDown("a"))
            {
                Mark();
            }

            if (Input.GetKeyDown("e"))
            {
                DeleteMark();
            }
        }

            //Progression spatiale du robot

            if (Mathf.Approximately(objectifZ, gameObject.transform.position.z) == false)
            {
                if (objectifZ - gameObject.transform.position.z < robotSpeed && (objectifZ - gameObject.transform.position.z) > 0)
                {
                    interPosition = gameObject.transform.position;

                    interPosition.z = objectifZ;
                    gameObject.transform.position = interPosition;
                }

                if (gameObject.transform.position.z - objectifZ < robotSpeed && (gameObject.transform.position.z - objectifZ) > 0)
                {
                    interPosition = gameObject.transform.position;

                    interPosition.z = objectifZ;
                    gameObject.transform.position = interPosition;
                }


                if (objectifZ > gameObject.transform.position.z)
                {
                    interPosition = gameObject.transform.position;

                    interPosition.z += robotSpeed;
                    gameObject.transform.position = interPosition;
                }
                else if (objectifZ < gameObject.transform.position.z)
                {

                    interPosition = gameObject.transform.position;

                    interPosition.z -= robotSpeed;
                    gameObject.transform.position = interPosition;
                }

                //animation de marche du robot
                anim.SetBool("Walk_Anim", true);
            }


            if (Mathf.Approximately(objectifX, gameObject.transform.position.x) == false)
            {
                if (objectifX - gameObject.transform.position.x < robotSpeed && (objectifX - gameObject.transform.position.x) > 0)
                {
                    interPosition = gameObject.transform.position;

                    interPosition.x = objectifX;
                    gameObject.transform.position = interPosition;
                }

                if (gameObject.transform.position.x - objectifX < robotSpeed && (gameObject.transform.position.x - objectifX) > 0)
                {
                    interPosition = gameObject.transform.position;

                    interPosition.x = objectifX;
                    gameObject.transform.position = interPosition;
                }

                if (objectifX > gameObject.transform.position.x)
                {
                    interPosition = gameObject.transform.position;

                    interPosition.x += robotSpeed;
                    gameObject.transform.position = interPosition;
                }
                else if (objectifX < gameObject.transform.position.x)
                {

                    interPosition = gameObject.transform.position;

                    interPosition.x -= robotSpeed;
                    gameObject.transform.position = interPosition;
                }

                anim.SetBool("Walk_Anim", true);
            }

            // si le robot ne bouge plus
            if (Mathf.Approximately(objectifX, gameObject.transform.position.x) && Mathf.Approximately(objectifZ, gameObject.transform.position.z))
            {
                anim.SetBool("Walk_Anim", false);
            } 
    }

    /// <summary>
    /// Fonction regroupant toutes les vérifications qu'on peut faire
    /// </summary>
    public void verifyAround()
    {
        if(IsInFrontOfDanger() == false)
        {
            isFrontOfMark();    
            IsFrontOfOre();      
        }

        IsOnMark();
        IsOnOre();

    }

    /// <summary>
    /// Si le robot est sorti de la map, il peut exploser lorsqu'il tombe (entre en collision avec un objet)
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(dead)
        {
            ParticleSystem myParticleExplosion = Instantiate(explosionEffect, new Vector3(robotTransform.position.x, robotTransform.position.y, robotTransform.position.z), Quaternion.Euler(0, 0, 0));
            explosionEffect.Play();

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// accesseur simple
    /// </summary>
    public void setDead()
    {
        dead = true;  
    }

    /// <summary>
    /// Rassemble les informations qu'on passe du manager au robot.
    /// </summary>
    public void Initialize(realPlanet thePlanet, ParticleSystem explosionEffect, float robotSpeed)
    {
        SetPlanet(thePlanet);
        SetSpeed(robotSpeed);

        this.explosionEffect = explosionEffect;

        
    }
}
