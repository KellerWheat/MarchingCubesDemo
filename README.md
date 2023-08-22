# MarchingCubesDemo
This is an implementation of the Marching Cubes algorithm in C# / Unity. I made it around a year ago to help a friend with a project he was working on.

[Marching Cubes](https://dl.acm.org/doi/10.1145/37402.37422) is an algorithm that converts a 3-dimensional array of voxels into a smooth mesh. It was originally used for medical imaging but 
has significant applications in both 3d modeling and game development. The implementation can be found in MarchingCubes.cs. The purpose of the other files is to use Perlin Noise to generate example terrains to run Marching Cubes on, and to texturize and display the remaining meshes.

If you stumble upon this feel free to use it for any purpose, and let me know if you have any questions about the implementation or need help.
