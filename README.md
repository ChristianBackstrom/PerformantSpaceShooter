# PerformantSpaceShooter
## This Project

This is a space shooter game that intends to be very performant and be able to have a lot of asteroids and projectiles at the same time on the screen. The project is done using Unity with ECS (Entity Component System) and DOTS (DATA-ORIENTED TECHNOLOGY STACK). ECS and Dots is a way to use Unity in a data-oriented way. What makes this different from normal Unity is that it stores data very compactly so that it is very easy to get acces to this data. This in return gives great performance but the code becomes a lot more boilerplaty.

## Inputs

A/D - Rotate ship

W - move the ship forward

Left click - Shoot

1 - restart/start ECS Game

Escape - Quit game

## Optimization

### [Unity.Physics]([https://link-url-here.org](https://github.com/ChristianBackstrom/PerformantSpaceShooter/releases/tag/v0.5.1))
Originally for this game I did distance check between projectile and asteroids by getting their respective aspects using Dots SystemAPI.Query. Using the profiler i realised this was taking about 50% of the cpu power since I got way more data than I would need. To improve upon this system and make the game more efficent I came up with a theory that using Unity.Physics package which would allow me to use trigger events using their collision system would be much faster since I would not need to check distance every frame. I implemented this into the game and did some testing using the profiler. The result from this experiment was that I got way worse performance so I did some more digging regarding why it was so slow in the profiler. It was the fact that it checked all the bounds of all the colliders which in return yielded way more computation power to be needed for the collision check. I did get more accurate collision but I decided it was not worth the performance loss. In the profiler I could see that the collisionhandling was taking about 90% of cpu power to do the collision handling.

![image](https://github.com/ChristianBackstrom/PerformantSpaceShooter/assets/54669344/f23526c5-8f3b-4248-8a57-22c3bd04e871)
The first spike is from when the objects are instantiating and being moved. The second spike happended when the 100 Projectiles and 100 Asteroids collided. 
![image](https://github.com/ChristianBackstrom/PerformantSpaceShooter/assets/54669344/37cb846e-4faa-4e37-bfc5-9eb0bfdec015)
If we look at the first spike we can see that it is the phsyicsgroup updating and waiting for jobs to complete.

#### Test options and result for using Unity.Physics:

100 projectiles

100 asteroids

1 player

fps: ~7.5

### [Distance Check]([https://link-url-here.org](https://github.com/ChristianBackstrom/PerformantSpaceShooter/releases/tag/v0.5.2))
After I tried the colliders I decided that I would redo the distance type collision check, but now that I had more knowledge and understanding of how ECS and Dots worked I could make it faster. In this version of the distance check I was aware that I should try and get only the data I needed. In the query I instead of getting the whole aspect with all the connected data I instead added tags to the entities and only got the location and entity which had that certain tag. This resulted in way less computing time and power which was needed to complete the distance check calculations.
![image](https://github.com/ChristianBackstrom/PerformantSpaceShooter/assets/54669344/0c1d4d26-9ace-4e4d-8eb4-c404bc796ad9)
![image](https://github.com/ChristianBackstrom/PerformantSpaceShooter/assets/54669344/7423cd03-27df-4067-b8b8-d61b973331b1)


#### Test options and result for using improved Distance Check:

1000 Asteroids

1000 Projectiles

1 Player

fps: ~140
