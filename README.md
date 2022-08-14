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
- ```Roll``` mode is still being worked on. You can spin through a bunch and land on a random avatar.


## Layers and Heirechy



We set up each layer, the probibility (Weights) or chance the layers show up, all of the meta-data for the JSON output, and references to the 3d layers that will be randomized. There is also 2 examples of scripts that have been customized to randomize materials on a mesh. (Background and Body)

![](https://i.imgur.com/yFDw64c.png)

1. Each Layer has a configuration script where you set up all your ```Values and Weights``` for each variation in a layer. 

![](https://i.imgur.com/OOB8U85.png =500x600)

3. Drag references to 3d gameObjects / layers into the ``"Total Objects In Layer"`` array. 

3. Setup the ``Layer String Data`` by matching the names of the values of the layers, that will be used to create the string JSON output.


 
Then the Random ```once, batch or roll``` can be chosen to trigger the randomization.

![](https://i.imgur.com/rIEIeJS.png)
![](https://i.imgur.com/CQ0Q5j8.png)

For the JSON string output, it is created and formatted in the script ```DNAManager```  

All of the output goes to the StreamingAssets folder in the Assets folder. Inside you will find the VRMs, JSON, and Images that were generated.

Change the Format dropdown to chage the output format, then click Export to export a single generated instance, or configure the tool to do a batch export! (Make sure you have enough hard drive space and do small tests first)

![](https://i.imgur.com/Dgi5rp6.png)

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


