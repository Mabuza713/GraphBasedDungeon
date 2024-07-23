# 3D Random Room Layout Generator

This project aims to generate random room layouts in a three-dimensional space. This system is particularly useful for creating diverse structures without manual modeling, making it ideal for applications like video games where each session can offer unique experiences to players.

## Table of Contents
- [Introduction](#introduction)
- [Problem Description](#problem-description)
- [3D Grid Creation](#3d-grid-creation)
- [Room Generation](#room-generation)
- [Graph Vertex Connection](#graph-vertex-connection)
- [Connection Reduction](#connection-reduction)
- [Pathfinding](#pathfinding)
- [Conclusion](#conclusion)
- [References](#references)

## Introduction
The purpose of this project is to demonstrate a method for connecting randomly placed rooms in a 3D space. The process uses the Unity engine and C# programming language to automate structure generation and visualize results.

## Problem Description
Generating a random layout of rooms and simulating them in 3D poses several challenges:
- Generated structures must form a cohesive whole. Rooms and pathways must not intersect, allowing free player movement.
- The map must offer interesting and unique experiences each time it is generated.

To meet these requirements, the concept of a graph is used where rooms are nodes and connections between them are edges, facilitating easy navigation and management of the space.

## 3D Grid Creation
To organize the operational space, a 3D array is used to store data about all possible vertices. The `CreateGrid` function initializes vertices in a grid with user-defined parameters, assigning positions both in the grid and in space.

### Key Functions
- **NodeFromWorldPosition**: Parses data about the vertex's position in the world to its position in the 3D grid.
- **GetNeighboringCells**: Returns a list of vertices directly adjacent to a selected vertex.

## Room Generation
Rooms are generated from a predefined list of templates to maintain control over their appearance, ensuring scalability for future project expansions. The `Room` class includes information about room size and available exits/entrances.

### Steps
1. Define the maximum number of rooms and the area where they can be placed, ensuring a minimum distance between rooms.
2. Select a template and a random location in space.
3. Use `CheckBoundaries` to ensure rooms do not overlap or exceed the defined area.
4. Create an instance of the room and map its exits to corresponding vertices using `NodeFromWorldPosition`.
5. Redefine properties of vertices within the room's dimensions to ensure correct corridor generation.

## Graph Vertex Connection
To ensure proper paths between rooms, the center coordinates of spheres passing through four different rooms are calculated. If the center is empty, mutual edges between vertices (rooms) are created.

### Key Functions
- **AddNodesToLinkedNodesList**: Links vertices.
- **CalculateCircumsphere**: Uses a 4x4 matrix to solve the sphere equation and determine valid room quartets for edge creation.

## Connection Reduction
A modified Prim’s algorithm is used to reduce connections, resulting in a minimal spanning tree. The algorithm adds the first vertex from `nodeList` and creates edges until the number of edges equals the number of rooms minus one.

### Key Functions
- **CurrentPossibleEdges**: Searches for and creates the edge with the smallest weight (distance).

## Pathfinding
The A* algorithm is used to find paths between vertices. The `TracePath` function reconstructs the path, placing corridor instances at appropriate vertices and setting stairs for vertical movement. 

### Key Functions
- **DeleteWalls**: Detects and removes walls between adjacent vertices to allow passage.

## Conclusion
The implementation of this system for generating random room layouts can serve as a foundation for creating varied and engaging maps, useful in video games to offer unique player experiences in each session.

## References
- [Procedurally Generated Dungeons](https://vazgriz.com/119/procedurally-generated-dungeons/)
- [Sebastian Lague's YouTube Series](https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW)
- [MathWorld: Circumsphere](https://mathworld.wolfram.com/Circumsphere.html)
