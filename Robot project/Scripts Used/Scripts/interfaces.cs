using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumeration pour les directions du robot, réutilisée dans les autres classes
/// </summary>
public enum Direction { North, East, South, West };

/// <summary>
/// Enumeration pour les coins possibles pour la position initiale du robot.
/// Réutilisée dans les autres classes
/// </summary>
public enum Corner { NorthEast, NorthWest, SouthEast, SouthWest };


/// <summary>
/// Interface commune avec la version 2D du jeu.
/// </summary>
public interface Robot
{
    void Move();
    void Rotate();
    void Mark();
    void DeleteMark();
    bool isFrontOfMark();
    bool IsOnMark();
    bool IsFrontOfOre();
    bool IsOnOre();
    bool IsInFrontOfDanger();
    float Speed();
    void SetSpeed(float speed);
    bool isBroken();
    void SetPlanet(Planet planet);
}


/// <summary>
/// Interface commune avec la version 2D du jeu
/// </summary>
public interface Planet
{
    void BoardSetup(int spawnID);
    Tuple<int, int> Dimension();
    void SetDimension(int column, int row);
    Direction Orientation(Robot robot);
    void SetOrientation(Robot robot, Direction orientation);
    Vector2 Position(Robot robot);
    void SetPosition(Robot robot, Vector2 position);
    Cell GetCell(int column, int row);
    void Initialize(Initialization init);
}

public interface Cell
{

}
