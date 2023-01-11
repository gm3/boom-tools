# boom-tools: a.r.m. avatar randomizer machine
###### tags: `boom-tools` `Unity Layer Randomizer` `devlog` ```v.0.0.1-alpha```

Wiki / API Docs https://github.com/gm3/boom-tools/wiki


![](https://hackmd.io/_uploads/r1tnnr45i.png)

## Summary
This is a tool that can randomize layers in Unity using weighted randomization. It can export VRMs using uniVRM. The tool can be configured to export a batch of VRMs with erc-1155 metadata, along with an image. You can use this tool to randomize pretty much anything if customized. 
![](https://hackmd.io/_uploads/rknRhrEqo.gif)

You can use VRMs in Webaverse, Hyperfy, Monaverse, Unreal Engine, Unity3d and tons of other places on the web!
![](https://hackmd.io/_uploads/SktZarEqo.gif)
![](https://hackmd.io/_uploads/SyYt6rN9j.gif)

## Dependencies

- This project was made with Unity 2021.3.6f1 
- https://unity3d.com/get-unity/download
- This project uses the `uniVRM` package for runtime VRM export
- https://github.com/vrm-c/UniVRM

## Instructions
- Push ```spacebar``` to randomize a layer, or click Random All
### Exporting Single Screen Shot
- Push F2 to Export iamge from The "Export Camera" to file.
### Batch Export
- In Batch mode you can export a VRM, the JSON, and an image, total of X times you can set in the batch export configuration script.
### Single Export
- You can also export a single VRM as well as just the JSON if you wish.
### Roll Mode (beta)
- ```Roll``` mode is still being worked on. You can spin through a bunch and land on a random avatar.


## Layers and Heirechy
![](https://i.imgur.com/uwNNh34.png)
All of the scripts are nested under the `_BoomTools` GameObject and the output can be configered there for the batch export.

## Setting up Layers
* Configure the weights for each layer in the inspector panel on the DNAManager sript
* Configure corresponding value associated with the weights
* Define the string data the meta-data the JSON output will contain
* Assign references to the 3d layers that will be randomized. 

---

1. Each Layer has a configuration script where you set up all your ```Values and Weights``` for each variation in a layer. 

![](https://i.imgur.com/OOB8U85.png)

2. Drag references to 3d gameObjects / layers into the ``"Total Objects In Layer"`` array. 

3. Setup the ``Layer String Data`` by matching the names of the values of the layers, that will be used to create the string JSON output.


Then the Random ```once, batch or roll``` can be chosen to trigger the randomization.


For the JSON string output, it is created and formatted in the script ```DNAManager```  

All of the output goes to the StreamingAssets folder in the Assets folder. Inside you will find the VRMs, JSON, and Images that were generated.

Change the Format dropdown to chage the output format, then click Export to export a single generated instance, or configure the tool to do a batch export! (Make sure you have enough hard drive space and do small tests first)

![](https://i.imgur.com/Dgi5rp6.png)

## Options Manager
In addition to the layer system, the new option manager allows you to select multiple options such as gameObjects, Textures and Materials in a quick way, use this method in case you require alot of elements to add

To start, open the Character Builder scene, and select _BoomTools from the hierarchy:

![image](https://user-images.githubusercontent.com/1117257/190047442-feb8f429-5580-4148-8dd4-ac8d3fda79c6.png)

To the left in the Inspector, you will see the options manager that will guide you through the process of creating randomizable vrm characters. Simply click on the Main Models option to start selecting your 3d models, the manager will automatically unlock the options and guide you in each step.

![image](https://user-images.githubusercontent.com/1117257/190047962-e35c6e3f-bcd4-4452-b096-af01dda1e0a2.png)

For further information refer to the wiki for a more detailed description of each step.

## Issues
https://github.com/gm3/boom-tools/issues

## License
This project is available under the MIT CC0 licence 

![](https://i.imgur.com/pYkqt1h.png)

## Contact
@boomboxheads http://www.twitter.com/boomboxheads


![](https://hackmd.io/_uploads/HkGwhBhqj.png)
