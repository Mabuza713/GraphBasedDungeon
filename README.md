Simple implementation of graph based dungeon generator in unity, it's not optimal, but can be used as building block of a project.
I have used a cicumspheres of four points to calculate tetrahedrons with vertecies as rooms. Then it calculates minimum spaning tree with Prim algorithm, and lastly find
halways using modified A* path finding. Main problem of this code is surely optimalization, I think it could be running much better if we change our approach to generating prefabs on map.
