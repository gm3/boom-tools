# boom-tools
###### tags: `boom-tools` `Unity Layer Randomizer` `devlog` ```v.0.0.1-alpha```

![](https://hackmd.io/_uploads/SyI1s490o.gif)

Wiki / API Docs https://github.com/gm3/boom-tools/wiki

## Summary
This is a tool that can randomize layers in Unity using weighted randomization. It can export VRMs using uniVRM, as well as a posed GLB. 

The tool can be configured to export a batch of VRMs with ERC JSON metadata, along with an image, animation_url, and vrm_url. 

Boom-tools can also import batches of JSON, and manage the loading of these layers with the goal to compose 2d projects into 3d VRMs and provide updated JSON for the VRMs.

## Example Output
![](https://hackmd.io/_uploads/BJ01-5Q12.jpg)

## Example JSON Output
```json
{ 
"name": "Boomboxhead #11",
"created_by": "Boomboxhead",
"external_url": "https://twitter.com/boomboxheads",
"description": "Boomboxheads V2 is a collection of 3D generative VRM avatars on the Ethereum Network. The avatars are cc0, and were created with Boom-tools , an open-source project running on Unity3d that can generate VRM avatars along with the metadata and thumbnail.  This project was inspired by the Boomboxheads: Originals and Boomboxheads: Legends, which both laid the foundation for this project to come to life.",
"vrm_url": "ipfs://www.replacethis.com/boomboxhead11.vrm",
"animation_url": "ipfs://www.replacethis.com/boomboxhead11.glb",
"image": "ipfs://www.replacethis.com/boomboxhead11.jpg",
"attributes": [
	{
		"trait_type": "Hat",
		"value": "Signal"
	},
	{
		"trait_type": "Mouth",
		"value": "Bandana"
	},
	{
		"trait_type": "Eyes",
		"value": "Clout"
	},
	{
		"trait_type": "Weapon",
		"value": "Axe"
	},
	{
		"trait_type": "Patches",
		"value": "AlteredEvil"
	},
	{
		"trait_type": "BodyTexture",
		"value": "Wild"
	},
	{
		"trait_type": "Pose",
		"value": "Dance1"
	},
	{
		"trait_type": "BGColor",
		"value": "White"
	},
	{
		"trait_type": "BBTexture",
		"value": "70ROY"
	},
	{
		"trait_type": "Border",
		"value": "BlueWall"
	}	]
}
```

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
- In Batch mode you can export a random VRM, along with a random posed GLB, the ERC JSON data, and an image, total of X times you can set in the batch export configuration script.


### Single Export
- You can also export a single VRM as well as just the JSON if you wish.


![](https://i.imgur.com/LNmNGJH.png)


## How to setup layers and traits



We set up each layer, the probibility (Weights) or chance the layers show up, all of the meta-data for the JSON output, and references to the 3d layers that will be randomized. There is also 2 examples of scripts that have been customized to randomize materials on a mesh. (Background and Body)

![](https://i.imgur.com/yFDw64c.png)

1. Each Layer has a configuration script where you set up all your ```Values and Weights``` for each variation in a layer. 

![](https://i.imgur.com/OOB8U85.png =500x600)

3. Drag references to 3d gameObjects / layers into the ``"Total Objects In Layer"`` array. 

3. Setup the ``Layer String Data`` by matching the names of the values of the layers, that will be used to create the string JSON output.


 
Then the Random ```once, batch or roll``` can be chosen to trigger the randomization.

![](https://i.imgur.com/rIEIeJS.png)
![](https://i.imgur.com/CQ0Q5j8.png)


## Mocap Tests

Here we bring in external live mocap to create animations using the Unity Recorder with a generated avatar. This animation was done by r00t, a friend of m3 and mvc

![](https://i.imgur.com/uLwjfwx.gif)

## License
This project is available under the MIT CC0 licence 

![](https://i.imgur.com/pYkqt1h.png)




