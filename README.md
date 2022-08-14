![](https://i.imgur.com/LNmNGJH.png)


# boom-tools: a.r.m. avatar randomizer machine
###### tags: `boom-tools` `Unity Layer Randomizer` `devlog` ```v.0.0.1-alpha```


![](https://i.imgur.com/qYqOZM5.gif)

![](https://i.imgur.com/s54Yj8F.png)


## Summary

This is a tool that can randomize layers in Unity, with a feature to export VRMs using uniVRM. The tool can be configured to export a batch of NFTs with erc-1155 metadata, and an image. 

## Dependencies

- This project was made with Unity 2021.3.6f1 
- https://unity3d.com/get-unity/download
- This project uses the `uniVRM` package for runtime VRM export
- https://github.com/vrm-c/UniVRM

## Instructions



- Push F2 to Export iamge from The "Export Camera" to file.
- In Batch mode you can export a VRM, the JSON, and an image, total of X times you can set in the batch export configuration script.
- You can also export a single VRM as well as just the JSON if you wish.
- Push ```spacebar``` to randomize a layer, or click Random All
- ```role``` mode is still being worked on. You can spin through a bunch and land on a random avatar.


## Layers and Heirechy

![](https://i.imgur.com/OOB8U85.png =500x600)

Configuring the randomimzer takes some getting used to, but essentially we are setting up each layer, the probibility (Weights) of the layers of showing up, all of the meta-data for the JSON output, and the 3d layers that will be randomized. There is also 2 layers that randomize materials on a mesh.

1. Each Layer has a configuration script where you set up all your ```Values and Weights``` for each variation in a layer. 

2. Drag references to 3d gameObjects / layers into the ``"Total Objects In Layer"`` array. 

3. Setup the ``Layer String Data`` by matching the names of the values of the layers, that will be used to create the string JSON output.

![](https://i.imgur.com/yFDw64c.png)
 
Then the Random Button Scripts trigger of a randomization of that layer, and if you hit randomize all it randomizes all layers, and creates a string in the script ```DNAManager``` to bring it all together. 

## About adding layers
Every layer you add you have to go through this process for now.
The material randomizer is slightly different as well and works with a different datatype (Material) so regarding data types, if you are to randomize new data types, they must be coded in. This is planned for future release. 



## Feature ideas

- Perfect looping video output with the Movie Recorder or ffmpeg.

## Mocap Tests

Here we bring in external live mocap to create animations using the Unity Recorder with a generated avatar. This animation was done by r00t, a friend of m3 and mvc

![](https://i.imgur.com/uLwjfwx.gif)

## License
This project is available under the MIT CC0 licence 

![](https://i.imgur.com/pYkqt1h.png)

## Contact
@godfreymeyer http://www.twitter.com/godfreymeyer


