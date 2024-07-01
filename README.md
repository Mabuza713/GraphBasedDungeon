Simple implementation of graph based dungeon generator in unity, it's not optimal, but can be used as building block of a project.
I have used a cicumspheres of four points to calculate tetrahedrons with vertecies as rooms. Then i would calculate minimum spaning tress with Prim algorithm, and lastly find
halway paths using modified A*. Main problem of this code is surely optimalization, I think it could be running much better if we change our approach to generating prefabs.
