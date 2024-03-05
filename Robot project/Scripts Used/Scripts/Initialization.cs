using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{

    public Vector2Int minDimensionMap;
    public Vector2Int maxDimensionMap;
    public Vector2Int initialPositionRobot;
    public bool isRandomInitialPosXRobot;
    public bool isRandomInitialPosYRobot;
    public Vector2Int positionDiamond;
    public bool randomPosDiamond;
    public Direction orientationRobot;
    public bool randomOrientationRobot;
    public bool isInCorner;
    public Corner cornerRobot;



    public Initialization(Vector2Int minDimensionMap, Vector2Int maxDimensionMap, Vector2Int initialPositionRobot,
    bool isRandomInitialPosXRobot, bool isRandomInitialPosYRobot, Vector2Int positionDiamond,
    bool randomPosDiamond, Direction orientationRobot, bool randomOrientationRobot,
    bool isInCorner, Corner cornerRobot)
    {
        this.minDimensionMap = minDimensionMap;
        this.maxDimensionMap = maxDimensionMap;
        this.initialPositionRobot = initialPositionRobot;
        this.isRandomInitialPosXRobot = isRandomInitialPosXRobot;
        this.isRandomInitialPosYRobot = isRandomInitialPosYRobot;
        this.positionDiamond = positionDiamond;
        this.randomPosDiamond = randomPosDiamond;
        this.orientationRobot = orientationRobot;
        this.randomOrientationRobot = randomOrientationRobot;
        this.isInCorner = isInCorner;
        this.cornerRobot = cornerRobot;
    }
}
